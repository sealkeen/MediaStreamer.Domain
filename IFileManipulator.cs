using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public interface IFileManipulator
    {
        Composition DecomposeAudioFile(string fileName, Action<string> errorAction = null);
        bool DecomposeAudioFiles(List<string> audioFiles, Action<string> errorAction);
        Composition CreateNewComposition(
            string fileName,
            string fullFileName,
            string titleFromFile,
            string artistFromFile,
            string genreFromFile,
            string albumFromFile,
            TimeSpan? duration,
            long? yearFromFile,
            Action<string> errorAction = null);

        /// <summary>
        /// Returns artist name and title from filename or matadata
        /// </summary>
        /// <param name="fileName">The file path in file system.</param>
        /// <param name="titleFromMetaD">title in metadata if presetnt</param>
        /// <param name="artistFromMetaD">artist in metadata if present</param>
        /// <param name="artistName">return value</param>
        /// <param name="compositionName">return value</param>
        /// <returns></returns>
        string ResolveArtistTitleConflicts(
            string fileName, string titleFromMetaD,
            string artistFromMetaD,
            ref string artistName, ref string compositionName);
    }
}
