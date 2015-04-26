namespace DocSearcher.Model
{
    public class ExtensionSpace
    {
        public string Extension { get; set; }

        public long Space { get; set; }
    }

    public class FloatExtensionSpace : ExtensionSpace
    {
        public new double Space { get; set; }
    }
}