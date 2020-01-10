# AtalhosExemplo

Este exemplo ensina como colocar atalhos no seu App Xamarin.Forms

<img src="https://github.com/TBertuzzi/AtalhosExemplo/blob/master/Resources/fotoIos.png?raw=true" alt="Smiley face" height="500" width="400">

Vamos ao Nuget Instalar o Plugin [Plugin.AppShortcuts](https://www.nuget.org/packages/Plugin.AppShortcuts/) em todos os projetos.

Em seguida no AppDelegate.cs do iOS Vamos Adicionar o Seguinte código :

```c#
  public override void PerformActionForShortcutItem(UIApplication application,
            UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
            var uri = Plugin.AppShortcuts.iOS.ArgumentsHelper.GetUriFromApplicationShortcutItem(shortcutItem);
            if (uri != null)
            {
                Xamarin.Forms.Application.Current.SendOnAppLinkRequestReceived(uri);
            }
        }
```

E no projeto Android Precisamos adicionar um IntentFilter com o nome do Aplicativo e um dataschema que 
utilizaremos para abrir as paginas dos atalhos no MainActivity.cs:

```c#
 [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryDefault },
              DataScheme = "atalho",
              DataHost = "atalhosexemplo",
              AutoVerify = true)]
```

Agora inicializamos o Controle com o CrossAppShortcuts.Current.Init();

```c#
  [Activity(Label = "AtalhosExemplo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryDefault },
              DataScheme = "atalho",
              DataHost = "atalhosexemplo",
              AutoVerify = true)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CrossAppShortcuts.Current.Init();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
```

Otimo ! Eu criei duas views bem simples para navegar os atalhos ViewAzul.xaml e ViewVerde.xaml agora podemos utilizar o plugin
no app.xaml.cs para nevegar por elas.

Vamos criar uma método chamado AdicionarAtalhos :

```c#
        async void AdicionarAtalhos()
        {
            if (CrossAppShortcuts.IsSupported)
            {

                var atalhos = await CrossAppShortcuts.Current.GetShortcuts();
                if (atalhos.FirstOrDefault(prop => prop.Label == "ViewAzul") == null)
                {
                    var shortcut = new Shortcut()
                    {
                        Label = "ViewAzul",
                        Description = "Ir para Pagina Azul",
                        Icon = new ContactIcon(),
                        Uri = $"{AppShortcutUriBase}{ShortcutOption1}"
                    };
                    await CrossAppShortcuts.Current.AddShortcut(shortcut);
                }

                if (atalhos.FirstOrDefault(prop => prop.Label == "ViewVerde") == null)
                {
                    var shortcut = new Shortcut()
                    {
                        Label = "ViewVerde 2",
                        Description = "Ir para Pagina Verde",
                        Icon = new UpdateIcon(),
                        Uri = $"{AppShortcutUriBase}{ShortcutOption2}"
                    };
                    await CrossAppShortcuts.Current.AddShortcut(shortcut);
                }

            }

        }
```

Este método serve para verificar se existem os atalhos e os adicionar caso não.

Agora basta configurarmos o OnAppLinkRequestReceived para quando recebermos um DeepLink ( do atalho no caso) ele 
redirecionar para a View Correta:


```c#

protected override void OnAppLinkRequestReceived(Uri uri)
        {
            var option = uri.ToString().Replace(AppShortcutUriBase, "");
            if (!string.IsNullOrEmpty(option))
            {
                MainPage = new NavigationPage(new MainPage());
                switch (option)
                {
                    case ShortcutOption1:
                        MainPage.Navigation.PushAsync(new ViewAzul());
                        break;
                    case ShortcutOption2:
                       
                        MainPage.Navigation.PushAsync(new ViewVerde());
                        break;
                }
            }
            else
                base.OnAppLinkRequestReceived(uri);
        }

```

Pronto basta rodar :

Caso fique a duvida este repositorio tem um exemplo da implementação completa. E tambem A Xamgirl ([Charlin Agramonte](https://github.com/Char0394)) escreveu um artigo bem 
legal sobre isso, e você pode conferir [Clicando Aqui](https://xamgirl.com/adding-shortcuts-in-xamarin-forms/)

Quer ver outros artigos sobre Xamarin ? [Clique aqui](https://github.com/TBertuzzi/XXamarin)

Espero ter ajudado!

Aquele abraço!
