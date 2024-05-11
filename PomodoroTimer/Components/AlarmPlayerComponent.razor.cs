using Howler.Blazor.Components;
using Howler.Blazor.Components.Events;
using Microsoft.AspNetCore.Components;

namespace PomodoroTimer.Components
{
    public partial class AlarmPlayerComponent
    {
        [Inject] private IHowl Howl { get; set; }
        [Inject] private IHowlGlobal HowlGlobal { get; set; }
        double volume = 1;
        int currentTaskId;

        protected override Task OnInitializedAsync()
        {
            Howl.OnPlay += HandleHowlEvent;
            return base.OnInitializedAsync();
        }

        protected void HandleHowlEvent(HowlEventArgs howlEvent)
        {
            if (howlEvent == null) { return; }

            currentTaskId = howlEvent.SoundId;
        }

        ~AlarmPlayerComponent()
        {
            Howl.OnPlay -= HandleHowlEvent;
        }

        public void Play()
        {
            var playOptions = new HowlOptions()
            {
                Loop = true,
                Sources = new string[] { "/timedone.wav" },
                Volume = volume
            };

            Howl.Play(playOptions);
        }

        protected void SamplePlay()
        {
            var playOptions = new HowlOptions()
            {
                Loop = false,
                Sources = new string[] { "/timedone.wav" },
                Volume = volume
            };

            Howl.Play(playOptions);
        }

        public async void Stop()
        {
            await Howl.Stop(currentTaskId);
        }
    }
}
