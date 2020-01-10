using System;
using System.Linq;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Icons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AtalhosExemplo
{
    public partial class App : Application
    {
        public const string AppShortcutUriBase = "asfs://appshortcutsformssample/";
        public const string ShortcutOption1 = "VIEWAZUL";
        public const string ShortcutOption2 = "VIEWVERDE";

        public App()
        {
            AdicionarAtalhos();

            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

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
    }
}
