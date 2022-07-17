using JsonDiffPatchDotNet;

using Xunit.Sdk;

namespace LexAssert.Exceptions
{
    public class JsonEqualException : AssertActualExpectedException
    {
        private string _message;

        public JsonEqualException(string expected, string actual)
            : base(expected, actual, "Lassert.JsonEqual() Failure")
        { }

        public override string Message
        {
            get { return _message ?? CreateMessage(); }
        }

        public string CreateMessage()
        {
            var magicDiffer = new JsonDiffPatch();
            var message = magicDiffer.Diff(Expected, Actual);
            _message = message;
            return message;
        }
    }
}
