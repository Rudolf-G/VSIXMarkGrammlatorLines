﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace VSIXMarkGrammlatorLines {
   /// <summary>
   /// Classification type definition export for EditorClassifier1
   /// </summary>
   internal static class GrammlatorClassifierClassificationDefinition {

      /// <summary>
      /// Defines the "GrammlatorClassifier" classification type.
      /// </summary>
      [Export(typeof(ClassificationTypeDefinition))]
      [Name("MarkGrammlatorLines")]
      // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169
      private static ClassificationTypeDefinition typeDefinition;
#pragma warning restore 169
      }
   }
