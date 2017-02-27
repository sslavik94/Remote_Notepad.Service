using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Remote_Notepad.Service.Domain
{
    public class UserInfo
    {
        /// <summary>
        ///     Gets or sets the Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the MessageCollection.
        /// </summary>
        public ObservableCollection<MessageInfo> MessageCollection { get; set; }

        public int Profile { get; set; }
    }
}
