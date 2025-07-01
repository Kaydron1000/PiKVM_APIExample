using PiKvmLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PiKVM_APIExample.ViewModels
{
    internal class ConfigurationViewModel : INotifyPropertyChanged
    {
        private ConfigurationData<PiKvmLibraryConfigurationType> _Configuration;
        private PiKvmLibraryConfigurationType _AppConfiguration;
        private ConnectionType _Connection;
        private PiKvmLibrary.Configuration.json.Streamer.Resolution _Resolution;

        private ObservableCollection<ConnectionType> _Connections;
        public ObservableCollection<ConnectionType> Connections 
        { 
            get => _Connections;
            set => SetField(ref _Connections, value);
        }

        private ConnectionType _SelectedConnection;
        public ConnectionType SelectedConnection
        {
            get => _SelectedConnection;
            set => SetField(ref _SelectedConnection, value);
        }
        public ConfigurationViewModel()
        {
            _Configuration = new ConfigurationData<PiKvmLibraryConfigurationType>();
            _AppConfiguration = _Configuration.ApplicationConfiguration;
        }


        #region Protected Methods
        /// <summary>
        /// Method that is used to invoke the PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Method that is used when a property value is changed to incoke the event PropertyChanged.
        /// </summary>
        /// <typeparam name="Ty">Type of the field that is being set.</typeparam>
        /// <param name="field">The field variable being set.</param>
        /// <param name="value">Value to set the field to.</param>
        /// <param name="propertyName">Property name assocaited with the field.</param>
        /// <returns>True, if OnPropertyChanged was called to invoke ProperyChanged event.</returns>
        protected virtual bool SetField<Ty>(ref Ty field, Ty value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<Ty>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /// <summary>
        /// Method that is used to invoke the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property name that was changed.</param>
        public virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
