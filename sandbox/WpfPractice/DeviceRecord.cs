using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
namespace WpfPractice
{
    public class DeviceRecord : BindableBase
    {
        public string Name { get; set; } = "";
        private string _location  = "";
        public string Location {
            get {
                return _location;
            }
            set {
                SetProperty(ref _location,value);
            }
        }
    }
}
