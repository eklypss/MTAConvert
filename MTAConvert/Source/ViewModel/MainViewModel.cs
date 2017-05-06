using MTAConvert.Command;
using MTAConvert.Converter;
using MTAConvert.Enum;
using Prism.Mvvm;
using System;
using System.Windows;

namespace MTAConvert.ViewModel
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

        /// <summary>
        /// Determines whether <see cref="Convert"/> can be executed based on settings defined by user.
        /// </summary>
        public bool CanConvert
        {
            get
            {
                if (!ConvertVehicles && !ConvertObjects && !ConvertRemovedObjects || ConvertType == ConvertType.NotSet || VehicleSpawnType == VehicleSpawnType.NotSet || DrawDistance <= 0 || StreamDistance < 0 || TextToConvert == string.Empty) return false;
                return true;
            }
        }

        /// <summary>
        /// Determines whether <see cref="ConvertedText"/> can be copied to clipboard with <see cref="Copy"/> or not.
        /// </summary>
        public bool CanCopy
        {
            get { return !string.IsNullOrWhiteSpace(ConvertedText); }
        }

        /// <summary>
        /// Copies <see cref="ConvertedText"/> to clipboard for easy copy-pasting.
        /// </summary>
        private void Copy()
        {
            Clipboard.SetText(ConvertedText);
        }

        /// <summary>
        /// TODO: Allow user to select a .map file instead of copy-pasting the source using OpenFileDialog.
        /// </summary>
        private void Browse()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calls <see cref="MapConverter"/> and attempts to convert the map given in <see cref="TextToConvert"/> and sets <see cref="ConvertedText"/> as the result.
        /// </summary>
        private async void Convert()
        {
            ConvertedText = await MapConverter.ConvertMap(TextToConvert, ConvertType, VehicleSpawnType, ConvertVehicles, ConvertObjects, ConvertRemovedObjects, StreamDistance, DrawDistance, AddComments);
        }

        /// <summary>
        /// Gets <see cref="ConvertType"/> for the specified index.
        /// </summary>
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

        /// <summary>
        /// Gets <see cref="VehicleSpawnType"/> for the specified index.
        /// </summary>
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
    }
}