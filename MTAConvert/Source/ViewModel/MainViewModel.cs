using MTAConvert.Command;
using MTAConvert.Converter;
using MTAConvert.Enum;
using Prism.Mvvm;
using System;
using System.Windows;

namespace MTAConvert.Source.ViewModel
{
    public class MainViewModel : BindableBase
    {
        public bool _convertObjects = true;
        public bool _convertVehicles = false;
        public bool _convertRemovedObjects = false;
        public float _streamDistance = 0.0f;
        public float _drawDistance = 300.0f;
        public int _selectedOutputIndex = -1;
        public int _selectedVehicleTypeIndex = -1;
        public bool _addComments = false;
        public string _textToConvert;
        public string _convertedText;

        public ConvertType ConvertType { get; set; }
        public VehicleSpawnType VehicleSpawnType { get; set; }
        public RelayCommand ConvertCommand { get; set; }
        public RelayCommand BrowseCommand { get; set; }
        public RelayCommand CopyCommand { get; set; }
        public MapConverter MapConverter { get; set; }

        public MainViewModel()
        {
            MapConverter = new MapConverter();
            ConvertCommand = new RelayCommand(Convert);
            BrowseCommand = new RelayCommand(Browse);
            CopyCommand = new RelayCommand(Copy);
        }

        #region Notifiers / binding properties

        public bool ConvertObjects
        {
            get { return _convertObjects; }
            set
            {
                SetProperty(ref _convertObjects, value, "ConvertObjects");
                RaisePropertyChanged("CanConvert");
            }
        }

        public bool ConvertRemovedObjects
        {
            get { return _convertRemovedObjects; }
            set
            {
                SetProperty(ref _convertRemovedObjects, value, "ConvertRemovedObjects");
                RaisePropertyChanged("CanConvert");
            }
        }

        public bool ConvertVehicles
        {
            get { return _convertVehicles; }
            set
            {
                SetProperty(ref _convertVehicles, value, "ConvertVehicles");
                RaisePropertyChanged("CanConvert");
            }
        }

        public float DrawDistance
        {
            get { return _drawDistance; }
            set
            {
                SetProperty(ref _drawDistance, value, "DrawDistance");
                RaisePropertyChanged("CanConvert");
            }
        }

        public float StreamDistance
        {
            get { return _streamDistance; }
            set
            {
                SetProperty(ref _streamDistance, value, "StreamDistance");
                RaisePropertyChanged("CanConvert");
            }
        }

        public int SelectedOutputIndex
        {
            get { return _selectedOutputIndex; }
            set
            {
                SetProperty(ref _selectedOutputIndex, value, "SelectedOutputIndex");
                ConvertType = GetConvertTypeFromIndex(value);
                RaisePropertyChanged("CanConvert");
            }
        }

        public int SelectedVehicleTypeIndex
        {
            get { return _selectedVehicleTypeIndex; }
            set
            {
                SetProperty(ref _selectedVehicleTypeIndex, value, "SelectedVehicleTypeIndex");
                VehicleSpawnType = GetVehicleSpawnTypeFromIndex(value);
                RaisePropertyChanged("CanConvert");
            }
        }

        public bool AddComments
        {
            get { return _addComments; }
            set
            {
                SetProperty(ref _addComments, value, "AddComments");
            }
        }

        public string TextToConvert
        {
            get { return _textToConvert; }
            set
            {
                SetProperty(ref _textToConvert, value, "TextToConvert");
                RaisePropertyChanged("CanConvert");
            }
        }

        public string ConvertedText
        {
            get { return _convertedText; }
            set
            {
                SetProperty(ref _convertedText, value, "ConvertedText");
                RaisePropertyChanged("CanCopy");
            }
        }

        public bool CanConvert
        {
            get
            {
                if (!ConvertVehicles && !ConvertObjects && !ConvertRemovedObjects || ConvertType == ConvertType.NotSet || VehicleSpawnType == VehicleSpawnType.NotSet || DrawDistance <= 0 || StreamDistance < 0 || TextToConvert == string.Empty) return false;
                return true;
            }
        }

        public bool CanCopy
        {
            get { return !string.IsNullOrWhiteSpace(ConvertedText); }
        }

        #endregion Notifiers / binding properties

        #region Commands

        private void Copy()
        {
            Clipboard.SetText(ConvertedText);
        }

        private void Browse()
        {
            throw new NotImplementedException();
        }

        private async void Convert()
        {
            ConvertedText = await MapConverter.ConvertMap(TextToConvert, ConvertType, VehicleSpawnType, ConvertVehicles, ConvertObjects, ConvertRemovedObjects, StreamDistance, DrawDistance, AddComments);
        }

        #endregion Commands

        #region Converters

        public ConvertType GetConvertTypeFromIndex(int index)
        {
            switch (index)
            {
                case -1: return ConvertType.NotSet;
                case 0: return ConvertType.Default;
                case 1: return ConvertType.Streamer;
                default: return ConvertType.NotSet;
            }
        }

        public VehicleSpawnType GetVehicleSpawnTypeFromIndex(int index)
        {
            switch (index)
            {
                case -1: return VehicleSpawnType.NotSet;
                case 0: return VehicleSpawnType.AddStaticVehicle;
                case 1: return VehicleSpawnType.AddStaticVehicleEx;
                case 2: return VehicleSpawnType.CreateVehicle;
                default: return VehicleSpawnType.NotSet;
            }
        }

        #endregion Converters
    }
}