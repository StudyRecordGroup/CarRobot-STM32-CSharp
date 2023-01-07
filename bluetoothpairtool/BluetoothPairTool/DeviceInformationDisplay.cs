using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace BluetoothPairTool
{
    public class DeviceInformationDisplay : INotifyPropertyChanged
    {
        public DeviceInformationDisplay(DeviceInformation deviceInfoIn)
        {
            DeviceInformation = deviceInfoIn;
            //UpdateGlyphBitmapImage();
            var address = deviceInfoIn.Id.Split('-').Last();
            Address = string.Join("", address.Split(':')).ToUpper();
        }
        public override string ToString()
        {
            return Name;
        }
        public string Address { get; private set; }
        public DeviceInformationKind Kind => DeviceInformation.Kind;
        public string Id => DeviceInformation.Id;
        public string Name => DeviceInformation.Name;
        //public BitmapImage GlyphBitmapImage { get; private set; }
        public bool CanPair => DeviceInformation.Pairing.CanPair;
        public bool IsPaired => DeviceInformation.Pairing.IsPaired;
        public bool IsConnected => Convert.ToBoolean(Properties["System.Devices.Aep.IsConnected"]);
        public string IsPresent => Properties["System.Devices.Aep.IsPresent"]?.ToString();
        public string Manufacturer => Properties["System.Devices.Aep.Manufacturer"]?.ToString();
        public string SignalStrength => Properties["System.Devices.Aep.SignalStrength"]?.ToString();
        public IReadOnlyDictionary<string, object> Properties => DeviceInformation.Properties;
        public DeviceInformation DeviceInformation { get; private set; }

        public string Status()
        {

            if (!Convert.ToBoolean(Properties["System.Devices.Aep.IsPresent"])) return "";
            if (IsConnected) return "Connected";
            if (IsPaired) return "Paired";
            if (CanPair) return "CanPair";
            return "unknow";
        }

        public void Update(DeviceInformationUpdate deviceInfoUpdate)
        {
            DeviceInformation.Update(deviceInfoUpdate);

            OnPropertyChanged("Kind");
            OnPropertyChanged("Id");
            OnPropertyChanged("Name");
            OnPropertyChanged("DeviceInformation");
            OnPropertyChanged("CanPair");
            OnPropertyChanged("IsPaired");
            OnPropertyChanged("IsConnected");
            OnPropertyChanged("IsPresent");
            OnPropertyChanged("Manufacturer");
            OnPropertyChanged("SignalStrength");
            OnPropertyChanged("GetPropertyForDisplay");

            //UpdateGlyphBitmapImage();
        }

        public string GetPropertyForDisplay(string key) => Properties[key]?.ToString();

        //private async void UpdateGlyphBitmapImage()
        //{
        //    DeviceThumbnail deviceThumbnail = await DeviceInformation.GetGlyphThumbnailAsync();
        //    BitmapImage glyphBitmapImage = new BitmapImage();
        //    await glyphBitmapImage.SetSourceAsync(deviceThumbnail);
        //    GlyphBitmapImage = glyphBitmapImage;
        //    OnPropertyChanged("GlyphBitmapImage");
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
