using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfTestApplication.Annotations;

namespace WpfTestApplication
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private string sampleText;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            SampleText = "Hello World!";
        }

        public string SampleText
        {
            get { return sampleText; }
            set
            {
                sampleText = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
