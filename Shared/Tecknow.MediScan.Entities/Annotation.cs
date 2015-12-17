using System.Collections.Generic;

namespace Tecknow.MediScan.Entities
{
    public class Annotation
    {
        public List<string> AnnotationComments { get; set; }

        public List<string> CommentDate { get; set; }

        public List<string> CommentTime { get; set; }
    }
}