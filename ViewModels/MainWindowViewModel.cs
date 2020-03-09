using NettyDemo.Commands;
using NettyDemo.network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.ViewModels {
    public class MainWindowViewModel : NotificationObject{

		ProtoBufSocket socket;
		
		private string inputText;

		public string InputText {
			get { return inputText; }
			set { inputText = value; this.RaisePropertyChanged("InputText"); }
		}

		private string receiveText;

		public string ReceiveText {
			get { return receiveText; }
			set { receiveText = value; this.RaisePropertyChanged("ReceiveText"); }
		}

		public DelegateCommand SendCommand { get; set; }

		public void Send(object parameter) {
			socket.SendMessage(CustomProtocol.Msg(CustomProtocol.Heartbeat().Id, InputText));
		}

		public MainWindowViewModel() {
			SendCommand = new DelegateCommand();
			SendCommand.ExcuteAction = new Action<object>(Send);

			socket = new ProtoBufSocket(this);
			socket.Connect();
		}

	}
}
