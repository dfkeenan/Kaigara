﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.ViewModels;
using ReactiveUI;

namespace Kaigara.MainWindow.ViewModels
{
    public class MainWindowViewModel : WindowViewModel
    {
        public MenuViewModel MainMenu { get; }

        public IShell Shell { get; }

        public MainWindowViewModel(MainMenuViewModel mainMenu, IShell shell)
        {
            this.MainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
            this.Shell = shell ?? throw new ArgumentNullException(nameof(shell));
        }

    }
}