using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Properties
{


    public class BodyFormatOfSQRP
    {
        private List<byte> bodyData;

        public BodyFormatOfSQRP()
        {
            bodyData = new List<byte>();
        }


        public List<byte> BodyData
        {
            get { return bodyData; }
        }


        public string ContentBodyOfSQRP
        {
            get { return System.Text.Encoding.ASCII.GetString(bodyData.ToArray()); }
        }


        public void AddaCharacter(char s)
        {
            byte[] BytesOfChar = System.Text.Encoding.ASCII.GetBytes(new char[] { s });
            bodyData.AddRange(BytesOfChar);
        }


        public void SetBody(string c1)
        {
            bodyData.Clear();
            byte[] contentBytes = System.Text.Encoding.ASCII.GetBytes(c1);
            bodyData.AddRange(contentBytes);
        }
    }
}
