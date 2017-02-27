namespace Remote_Notepad.Service.Domain
{
    public class MessageInfo
    {
        /// <summary>
        ///     Gets or sets the MsgName.
        /// </summary>
        public string MessageName { get; set; }


        /// <summary>
        ///     Gets or sets the MsgContent.
        /// </summary>
        public string MessageContent { get; set; }

        public MessageInfo(string messageName, string messageContent)
        {
            MessageName = messageName;
            MessageContent = messageContent;
        }
    }
}
