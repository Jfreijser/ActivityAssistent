using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ActivityAssistent.Shared.Enums
{
    public enum AiStatus
    {
        [Description("Request received, placed in queue.")]
        Queued = 0,

        [Description("Reading and preparing audio file...")]
        PreparingAudio = 10,

        // 2. Speech-to-Text (Azure)
        [Description("Securely uploading audio to speech service...")]
        UploadingToSpeechService = 20,

        [Description("Deciphering speech and converting to text...")]
        TranscribingAudio = 21,

        [Description("Transcription successfully received.")]
        TranscriptionCompleted = 22,

        // 3. AI Analysis (Action Points)
        [Description("Forwarding text to AI assistant...")]
        SendingToAiAssistant = 30,

        [Description("Analyzing conversation and generating action points...")]
        GeneratingActionPoints = 31,

        // 4. Storage 
        [Description("Securely saving results to the database...")]
        SavingToDatabase = 40,

        // 5. Completion
        [Description("Analysis fully and successfully completed!")]
        CompletedSuccessfully = 50,

        // 6. Error Handling
        [Description("An error occurred while preparing the audio.")]
        ErrorAudioProcessing = 900,

        [Description("An error occurred with the Azure speech service.")]
        ErrorTranscription = 901,

        [Description("An error occurred while generating AI action points.")]
        ErrorAiAnalysis = 902,

        [Description("An error occurred while saving the data.")]
        ErrorDatabaseSave = 903,

        [Description("An unknown system error has occurred.")]
        ErrorUnknown = 999


    }
}
