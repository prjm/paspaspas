using PasPasPas.Parsing.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasPasPasTests.Misc {

    public class TreeImplementation : ISyntaxPart {

        public TreeImplementation(string v) {
            Data = v;
        }

        public string Data { get; set; }

        public ISyntaxPart Parent { get; set; }

        public IList<ISyntaxPart> Children { get; }
            = new List<ISyntaxPart>();

        public IEnumerable<ISyntaxPart> Parts
            => Children;

        public bool Accept<T>(ISyntaxPartVisitor<T> visitor, T visitorParameter) {
            if (!visitor.BeginVisit(this, visitorParameter))
                return false;

            var result = true;

            foreach (var part in Parts) {
                visitor.BeginVisitChild(this, visitorParameter, part);
                result = result && ((TreeImplementation)part).Accept(visitor, visitorParameter);
                visitor.EndVisitChild(this, visitorParameter, part);
            }

            if (!visitor.EndVisit(this, visitorParameter))
                return false;

            return result;

        }

        public TreeImplementation AddChild(string v) {
            var result = new TreeImplementation(v);
            result.Parent = this;
            Children.Add(result);
            return result;
        }
    }

    public class VisitorOptions {

        public IList<string> History { get; }
            = new List<string>();

    }

    public class VisitorImplementation : ISyntaxPartVisitor<VisitorOptions> {

        public bool BeginVisit(ISyntaxPart syntaxPart, VisitorOptions parameter) {
            parameter.History.Add((syntaxPart as TreeImplementation).Data + 'B');
            return true;
        }

        public void BeginVisitChild(ISyntaxPart parent, VisitorOptions visitorParameter, ISyntaxPart child) {
            visitorParameter.History.Add("X" + (parent as TreeImplementation).Data + '.' + (child as TreeImplementation).Data);
        }

        public bool EndVisit(ISyntaxPart syntaxPart, VisitorOptions parameter) {
            parameter.History.Add((syntaxPart as TreeImplementation).Data + 'E');
            return true;
        }

        public void EndVisitChild(ISyntaxPart parent, VisitorOptions visitorParameter, ISyntaxPart child) {
            visitorParameter.History.Add('Y' + (parent as TreeImplementation).Data + '.' + (child as TreeImplementation).Data);
        }
    }

    public class VisitorTest {

        // 1BX1.22B2EY1.2X1.33BX3.55B5EY3.5X3.66B6EY3.6X3.77B7EY3.73EY1.3X1.44B4EY1.41E
        private TreeImplementation CreateSampleTree1() {
            var result = new TreeImplementation("1");
            var c1 = result.AddChild("2");
            var c2 = result.AddChild("3");
            var c3 = result.AddChild("4");

            var c4 = c2.AddChild("5");
            var c5 = c2.AddChild("6");
            var c6 = c2.AddChild("7");

            return result;
        }

        private TreeImplementation CreateSampleTree2() {
            var result = new TreeImplementation("1");
            return result;
        }

        private TreeImplementation CreateSampleTree3() {
            var result = new TreeImplementation("1");
            var c2 = result.AddChild("2");
            var c3 = c2.AddChild("2");
            var c4 = c3.AddChild("2");
            return result;
        }

        private TreeImplementation CreateSampleTree4() {
            var result = new TreeImplementation("1");
            var c2 = result.AddChild("2");
            var c3 = c2.AddChild("2");
            var c31 = c3.AddChild("31");
            var c32 = c3.AddChild("32");
            var c4 = c3.AddChild("2");
            return result;
        }

        private string Stringify1(TreeImplementation t) {
            VisitorImplementation v = new VisitorImplementation();
            VisitorOptions o = new VisitorOptions();
            t.Accept(v, o);
            return o.History.Aggregate((a, b) => string.Concat(a, b));
        }

        private string Stringify2(TreeImplementation t) {
            VisitorImplementation v = new VisitorImplementation();
            VisitorOptions o = new VisitorOptions();
            VisitorHelper.AcceptVisitor(t, v, o);
            return o.History.Aggregate((a, b) => string.Concat(a, b));
        }

        [Fact]
        public void TestAccept() {
            var s = Stringify1(CreateSampleTree1());
            Assert.IsTrue(s.Length > 0);
        }

        [Fact]
        public void TestHelper() {
            var data = CreateSampleTree1();
            var expected = Stringify1(data);
            var occured = Stringify2(data);
            Assert.Equals(expected, occured);

            data = CreateSampleTree2();
            expected = Stringify1(data);
            occured = Stringify2(data);
            Assert.Equals(expected, occured);

            data = CreateSampleTree3();
            expected = Stringify1(data);
            occured = Stringify2(data);
            Assert.Equals(expected, occured);

            data = CreateSampleTree4();
            expected = Stringify1(data);
            occured = Stringify2(data);
            Assert.Equals(expected, occured);
        }

    }
}
