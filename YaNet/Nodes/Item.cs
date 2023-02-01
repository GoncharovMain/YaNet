namespace YaNet.Nodes
{
    public class Item : INode
    {
        public INode Node { get; set; }
        public Item(INode node)
        {
            Node = node;
        }

        public void Init(ref object obj, StringBuilder buffer)
        {
            Node.Init(ref obj, buffer);
        }

        public void Print(StringBuilder buffer)
        {
            Node.Print(buffer);
        }
    }
}