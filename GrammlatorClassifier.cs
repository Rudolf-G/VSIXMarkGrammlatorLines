using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace VSIXMarkGrammlatorLines {

   /*
    * Advice how to program this extension: 
    * https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2019
    * https://docs.microsoft.com/en-us/visualstudio/extensibility/inside-the-editor?view=vs-2019
    */

   /// <summary>
   /// Classifier that classifies all text as an instance of the "EditorClassifier1" classification type.
   /// </summary>
   internal class GrammlatorClassifier: IClassifier {
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
         ITextSnapshot text = span.Snapshot;

         Int32 pos = -1;
         for (int i = span.Start; i < span.End - 3; i++)
            {
            if (char.IsWhiteSpace(text[i]))
               continue;
            if (text[i] == '/' && text[i + 1] == '/' && text[i + 2] == '|')
               pos = i;
            break;
            }

         if (pos < 0)
            return EmptyList;

         return new List<ClassificationSpan>() {
            new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(pos, 3)), this.classificationType)
            };
         }

      static List<ClassificationSpan> EmptyList = new List<ClassificationSpan>();

      #endregion
      }
   }
