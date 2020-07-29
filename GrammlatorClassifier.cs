using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace VSIXMarkGrammlatorLines {

   /*
    * Advice how to program this extension: 
    * https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2019
    * https://docs.microsoft.com/en-us/visualstudio/extensibility/inside-the-editor?view=vs-2019
    * https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-highlighting-text?view=vs-2019
    */

   /// <summary>
   /// Classifier that classifies all text as an instance of the "EditorClassifier1" classification type.
   /// </summary>
   internal class GrammlatorClassifier : IClassifier {
      /// <summary>
      /// Classification type.
      /// </summary>
      private readonly IClassificationType classificationType;

      /// <summary>
      /// Initializes a new instance of the <see cref="GrammlatorClassifier"/> class.
      /// </summary>
      /// <param name="registry">Classification registry.</param>
      internal GrammlatorClassifier(IClassificationTypeRegistryService registry)
      {
         this.classificationType = registry.GetClassificationType("MarkGrammlatorLines");
      }

      #region IClassifier

#pragma warning disable 67

      /// <summary>
      /// An event that occurs when the classification of a span of text has changed.
      /// </summary>
      /// <remarks>
      /// This event gets raised if a non-text change would affect the classification in some way,
      /// for example typing /* would cause the classification to change in C# without directly
      /// affecting the span.
      /// </remarks>
      public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67

      /// <summary>
      /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
      /// </summary>
      /// <remarks>
      /// This method scans the given SnapshotSpan for potential matches for this classification.
      /// In this instance, it classifies each occurenc of "//|" at the beginning of  line (ignoring whitespace)
      /// </remarks>
      /// <param name="span">The span currently being classified.</param>
      /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
      public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
      {
         // ITextSnapshot text = span.Snapshot; // The ITextSnapshot to which the SnapshotSpan span refers, a line.

         for (int i = span.Start; i < span.Snapshot.Length - 2; i++)
         {
            char c = span.Snapshot[i];

            if (char.IsWhiteSpace(c))
               continue; // skip whitespace

            if (c == '/' && span.Snapshot[i + 1] == '/' && span.Snapshot[i + 2] == '|')
            {  // found "//|"
               return
                  new List<ClassificationSpan>()
                  {
                     new ClassificationSpan(new SnapshotSpan(span.Snapshot, i, 3), this.classificationType)
                  };
            }

            break; // not found
         }

         return new List<ClassificationSpan> 
         { new ClassificationSpan(new SnapshotSpan(span.Snapshot, span.Start, 0), this.classificationType) }; // nothing to highlight test: 1 Span
      }

      /* Original code (highlighting all text) created as recommended in
       * https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-an-editor-item-template?view=vs-2019
       * var result = new List<ClassificationSpan>()
       *    {
       *    new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start, span.Length)), this.classificationType)
       *    };
       * return result;
       */
   }

   /*
    <description>System.ArgumentOutOfRangeException: Das angegebene Argument liegt außerhalb des gültigen Wertebereichs.
   Parametername: position
   bei Microsoft.VisualStudio.Text.SnapshotPoint..ctor(ITextSnapshot snapshot, Int32 position)
   bei VSIXMarkGrammlatorLines.GrammlatorClassifier.GetClassificationSpans(SnapshotSpan span) in C:\Users\Rolf\Documents\VisualStudioMeineProjekte\VSIXMarkGrammlatorLines\GrammlatorClassifier.cs:Zeile 63.

   bei Microsoft.VisualStudio.Text.Classification.Implementation.ClassifierTagger.<GetTags>d__5.MoveNext()
   bei Microsoft.VisualStudio.Text.Tagging.Implementation.TagAggregator`1.<GetTagsForBuffer>d__47.MoveNext()
   --- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei Microsoft.VisualStudio.Telemetry.WindowsErrorReporting.WatsonReport.GetClrWatsonExceptionInfo(Exception exceptionObject)</description>
    */

   // https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2019#extend-classification-types-and-classification-formats
   // https://docs.microsoft.com/en-us/visualstudio/extensibility/inside-the-editor?view=vs-2019
   // https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2019


   #endregion
}

