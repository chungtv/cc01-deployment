using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_Linq
{
    /// <summary>
    /// C'tor
    /// </summary>
    public partial class ucTabControl : UserControl
    {
        public ucTabControl()
        {
            InitializeComponent();
            //For the sample, the Window's DataContext is its code-behind.
            this.DataContext = this;
        }
        #region --- CloseCommand ---

        private Utils.RelayCommand _cmdCloseCommand;
        /// <summary>
        /// Returns a command that closes a TabItem.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_cmdCloseCommand == null)
                {
                    _cmdCloseCommand = new Utils.RelayCommand(
                        param => this.CloseTab_Execute(param),
                        param => this.CloseTab_CanExecute(param)
                        );
                }
                return _cmdCloseCommand;
            }
        }

        /// <summary>
        /// Called when the command is to be executed.
        /// </summary>
        /// <param name="parm">
        /// The TabItem in which the Close-button was clicked.
        /// </param>
        private void CloseTab_Execute(object parm)
        {
            TabItem ti = parm as TabItem;
            if (ti != null)
                tc.Items.Remove(parm);
        }

        /// <summary>
        /// Called when the availability of the Close command needs to be determined.
        /// </summary>
        /// <param name="parm">
        /// The TabItem for which to determine the availability of the Close-command.
        /// </param>
        private bool CloseTab_CanExecute(object parm)
        {
            //For the sample, the closing of TabItems will only be
            //unavailable for disabled TabItems and the very first TabItem.
            TabItem ti = parm as TabItem;
            //if (ti != null && ti != tc.Items[0])
            if (ti != null)
                //We have a valid reference to a TabItem, so return 
                //true if the TabItem is enabled.
                return ti.IsEnabled;

            //If no reference to a TabItem could be obtained, the command 
            //cannot be executed
            return false;
        }

        #endregion
    }
}
