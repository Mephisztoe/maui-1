﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppKit;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        static readonly Lazy<NSSpeechSynthesizer> speechSynthesizer = new Lazy<NSSpeechSynthesizer>(() =>
            new NSSpeechSynthesizer { Delegate = new SpeechSynthesizerDelegate() });

        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync() =>
            Task.FromResult(NSSpeechSynthesizer.AvailableVoices
                .Select(v => new Locale(v, null, null, null)));

        internal static async Task PlatformSpeakAsync(string text, SpeechOptions options, CancellationToken cancelToken = default)
        {
            var ss = speechSynthesizer.Value;
            var ssd = (SpeechSynthesizerDelegate)ss.Delegate;

            var tcs = new TaskCompletionSource<bool>();
            try
            {
                // Ensures linker doesn't remove.
                if (DateTime.UtcNow.Ticks < 0)
                {
                    new NSSpeechSynthesizer();
                    new SpeechSynthesizerDelegate();
                }

                if (options != null)
                {
                    if (options.Volume.HasValue)
                        ss.Volume = NormalizeVolume(options.Volume);

                    if (options.Locale != null)
                        ss.Voice = options.Locale.Language;
                }

                ssd.FinishedSpeaking += OnFinishedSpeaking;
                ssd.EncounteredError += OnEncounteredError;

                ss.StartSpeakingString(text);

                using (cancelToken.Register(TryCancel))
                {
                    await tcs.Task;
                }
            }
            finally
            {
                ssd.FinishedSpeaking -= OnFinishedSpeaking;
                ssd.EncounteredError -= OnEncounteredError;
            }

            void TryCancel()
            {
                ss.StopSpeaking(NSSpeechBoundary.hWord);
                tcs.TrySetResult(true);
            }

            void OnFinishedSpeaking(bool completed)
            {
                tcs.TrySetResult(completed);
            }

            void OnEncounteredError(string errorMessage)
            {
                tcs.TrySetException(new EssentialsException(errorMessage));
            }
        }

        static float NormalizeVolume(float? volume)
        {
            var v = volume ?? 1.0f;
            if (v > 1.0f)
                v = 1.0f;
            else if (v < 0.0f)
                v = 0.0f;
            return v;
        }

        class SpeechSynthesizerDelegate : NSSpeechSynthesizerDelegate
        {
            public event Action<bool> FinishedSpeaking;

            public event Action<string> EncounteredError;

            public override void DidEncounterError(NSSpeechSynthesizer sender, nuint characterIndex, string theString, string message) =>
                EncounteredError?.Invoke(message);

            public override void DidFinishSpeaking(NSSpeechSynthesizer sender, bool finishedSpeaking) =>
                FinishedSpeaking?.Invoke(finishedSpeaking);
        }
    }
}
