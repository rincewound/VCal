using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcal.Parser
{
    public class Tokenizer
    {
        string[] splitSigns;

        public Tokenizer(string[] splitSigns)
        {
            this.splitSigns = splitSigns;
        }

        public IEnumerable<string> Tokenize(string data)
        {
            var retVal = new List<string>();

            string accBuff = "";

            /*
             * The algorithm works by slowly accumulating each and every character
             * in the acc buffer, until a complete split sign is encountered. In this
             * case all data preceding the split sign are put into one token, the splitsign
             * itself is put into one token and the buffer is cleared to restart the process.
             * Note that this tokenizer will never filter the split signs.
             * */
            for(int i = 0; i < data.Length; i++)
            {
                accBuff += data[i];

                var theSign = splitSigns.FirstOrDefault(x => accBuff.Contains(x));

                if (default(string) == theSign)
                    continue;

                var preSign = accBuff.Take(accBuff.Length - theSign.Length);
                retVal.Add(new string(preSign.ToArray()));
                retVal.Add(theSign);
                accBuff = "";

            }

            if (!string.IsNullOrWhiteSpace(accBuff))
                retVal.Add(accBuff);

            return retVal;
        }
    }
}
