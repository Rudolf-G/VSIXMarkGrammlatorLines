using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace VSIXMarkGrammlatorLines {
   /// <summary>
   /// Defines an editor format for the GrammlatorClassifier type that displays the grammlator
   /// line marks "//|" with a coloured background
   /// and is underlined.
   /// </summary>
   [Export(typeof(EditorFormatDefinition))]
   [ClassificationType(ClassificationTypeNames = "MarkGrammlatorLines")]
   [Name("MarkGrammlatorLines")]
   [UserVisible(true)] // This should be visible to the end user
   [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
   internal sealed class GrammlatorClassifierFormat: ClassificationFormatDefinition {
      /// <summary>
      /// Initializes a new instance of the <see cref="GrammlatorClassifierFormat"/> class.
      /// </summary>
      public GrammlatorClassifierFormat()
         {
         // colors see https://docs.microsoft.com/de-de/dotnet/api/system.windows.media.colors?view=netcore-3.1
         this.DisplayName = "MarkGrammlatorLines"; // Human readable version of the name
         this.BackgroundColor = Colors.PowderBlue;
         // this.TextDecorations = System.Windows.TextDecorations.Underline;
         }
      }
   }
