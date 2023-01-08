namespace Universum {
    public static class Logger {
        public static void print(Importance importance, string key, bool prefix = true, Verse.NamedArgument[] args = null) {
            string message;
            if (args == null) {
                message = Verse.TranslatorFormattedStringExtensions.Translate(key);
            } else {
                message = Verse.TranslatorFormattedStringExtensions.Translate(key, args);
            }
            print(importance, text: message, prefix);
        }

        public static void print(Importance importance, string text, bool prefix = true) {
            if (prefix) text = Info.name + ": " + text;
            switch (importance) {
                case Importance.Info:
                    Verse.Log.Message(text);
                    return;
                case Importance.Warning:
                    Verse.Log.Warning(text);
                    return;
                case Importance.Error:
                    Verse.Log.Error(text);
                    return;
            }
        }
    }

    public enum Importance {
        Info = 0,
        Warning = 1,
        Error = 2,
    }
}
