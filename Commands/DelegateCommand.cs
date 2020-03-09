using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NettyDemo.Commands {
    public class DelegateCommand : ICommand {
        public event EventHandler CanExecuteChanged;

        public Action<Object> ExcuteAction { get; set; }
        public Func<Object, bool> CanExcuteFunc { get; set; }

        public bool CanExecute(object parameter) {

            if (this.CanExcuteFunc == null) {
                return true;
            }
            return this.CanExcuteFunc(parameter);
        }

        public void Execute(object parameter) {
            if (this.ExcuteAction == null) {
                return;
            }
            this.ExcuteAction(parameter);
        }
    }
}
