using AISOS.Commands;
using AISOS.Data;
using AISOS.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AISOS.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private string welcomeText;
        private BaseCommand openUsersCommand;
        private BaseCommand openDutyCommand;
        private BaseCommand exitCommand;
        private BaseCommand openInfoCommand;
        private readonly MenuWindow menuWindow;

        public string WelcomeText
        {
            get => welcomeText; set
            {
                welcomeText = value;
                OnPropertyChanged("WelcomeText");
            }
        }

        public User CurrentUser => Constants.CurrenUser;
        public Visibility AdminVisibility { get; set; } = Visibility.Collapsed;

        public MenuViewModel(MenuWindow menuWindow)
        {
            this.menuWindow = menuWindow;
            if (Constants.CurrenUser.IsAdmin) AdminVisibility = Visibility.Visible;
            WelcomeText = $"Добро пожаловать,\n{CurrentUser.SecondName} {CurrentUser.FirstName} {CurrentUser.ThirdName}";
        }

        public BaseCommand ExitCommand
        {
            get => exitCommand ??
                (exitCommand = new BaseCommand(obj =>
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    menuWindow.Close();
                }));
        }

        public BaseCommand OpenUsersCommand
        {
            get => openUsersCommand ??
                (openUsersCommand = new BaseCommand(obj =>
                {
                    UsersWindow usersWindow = new UsersWindow();
                    usersWindow.Show();
                    menuWindow.Close();
                }));
        }
        
        public BaseCommand OpenDutyCommand
        {
            get => openDutyCommand ??
                (openDutyCommand = new BaseCommand(obj =>
                {
                    DutyWindow dutyWindow = new DutyWindow();
                    dutyWindow.Show();
                    menuWindow.Close();
                }));
        }

        public BaseCommand OpenInfoCommand
        {
            get => openInfoCommand ??
                (openInfoCommand = new BaseCommand(obj =>
                {
                    AddUserWindow addUserWindow = new AddUserWindow(CurrentUser);
                    addUserWindow.ShowDialog();
                }));
        }
    }
}
