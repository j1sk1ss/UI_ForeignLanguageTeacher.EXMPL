using System.Collections.Generic;

namespace UI_ForeignLanguageTeacher.EXMPL.Objects {
    public class Quest {
        public Quest() {
            Questions = new List<string>();
            Answers = new List<string>();
        }
        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }
    }
}