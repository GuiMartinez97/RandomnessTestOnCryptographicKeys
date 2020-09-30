using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RandomnessTestOnCryptographicKeys
{
    class Program
    {
        static void Main(string[] _args)
        {
            string allKeysInStringFormat = File.ReadAllText("Chaves de Criptografia.txt");
            string[] sigleQuoteSplitResult = allKeysInStringFormat.Split('\'');
            var keys = new List<string>();

            foreach (string keyCandidate in sigleQuoteSplitResult)
            {
                if (keyCandidate != null && keyCandidate != "" && !keyCandidate.Contains("\n"))
                {
                    keys.Add(keyCandidate);
                }
            }

            File.Delete("Result.txt");

            using (var sw = new StreamWriter("Result.txt"))
            {
                sw.WriteLine($"https://github.com/GuiMartinez97/RandomnessTestOnCryptographicKeys");
                sw.WriteLine("");

                foreach (string key in keys)
                {
                    string keyInBinaryFormat = StringToBinary(key);
                    string first20KInKeyInBinaryFormat = keyInBinaryFormat.Substring(0, 20000);
                    bool monobitTestResult = MonobitTest(first20KInKeyInBinaryFormat);
                    bool pokerTestResult = PokerTest(first20KInKeyInBinaryFormat);
                    bool runsTestResult = RunsTest(first20KInKeyInBinaryFormat);

                    sw.WriteLine($"KEY: {key}");
                    sw.WriteLine($"Monobit Test Result: {monobitTestResult}");
                    sw.WriteLine($"Poker Test Result: {pokerTestResult}");
                    sw.WriteLine($"Runs Test Result: {runsTestResult}");
                    sw.WriteLine("");
                }
            }
        }

        static bool MonobitTest(string _key)
        {
            int onesCount = _key.Count(c => c == '1');
            if (onesCount > 9654 && onesCount < 10346)
            {
                return true;
            }

            return false;
        }

        static bool PokerTest(string _key)
        {
            IEnumerable<string> keyChunks = Split(_key, 4);
            var possible4BitValuesOccurrences = new Dictionary<string, int>();

            foreach (string keyChunk in keyChunks)
            {
                AddOneInOccurrence(possible4BitValuesOccurrences, keyChunk);
            }

            double occurrenceSumSquaredAcumulator = 0;

            foreach (KeyValuePair<string, int> possible4BitValuesOccurrence in possible4BitValuesOccurrences)
            {
                double occurrenceSumSquared = Math.Pow(possible4BitValuesOccurrence.Value, 2);
                occurrenceSumSquaredAcumulator += occurrenceSumSquared;
            }

            double pokerTestValue = (16.0 / 5000.0) * occurrenceSumSquaredAcumulator - 5000.0;

            if (pokerTestValue > 1.03 && pokerTestValue < 57.4)
            {
                return true;
            }

            return false;
        }

        static bool RunsTest(string _key)
        {
            var runsOccurrences = new Dictionary<string, int>();
            int currentLength = 0;
            char currentBit = _key[0];

            for (int i = 0; i< _key.Length; i++)
            {
                if(currentBit == _key[i])
                {
                    currentLength++;
                }
                else
                {
                    AddOneInOccurrence(runsOccurrences, currentLength.ToString());
                    currentLength = 1;
                    currentBit = _key[i];
                }
            }

            foreach (KeyValuePair<string, int> runsOccurrence in runsOccurrences)
            {
                switch (runsOccurrence.Key)
                {
                    case "1":
                        {
                            if(runsOccurrence.Value < 2267 || runsOccurrence.Value > 2733)
                            {
                                return false;
                            }
                            break;
                        }
                    case "2":
                        {
                            if (runsOccurrence.Value < 1079 || runsOccurrence.Value > 1421)
                            {
                                return false;
                            }
                            break;
                        }
                    case "3":
                        {
                            if (runsOccurrence.Value < 502 || runsOccurrence.Value > 748)
                            {
                                return false;
                            }
                            break;
                        }
                    case "4":
                        {
                            if (runsOccurrence.Value < 223 || runsOccurrence.Value > 402)
                            {
                                return false;
                            }
                            break;
                        }
                    case "5":
                    default:
                        {
                            if (runsOccurrence.Value < 90 || runsOccurrence.Value > 223)
                            {
                                return false;
                            }
                            break;
                        }
                }
            }

            return true;
        }

        static void AddOneInOccurrence(Dictionary<string, int> _occurrencesDictionary, string _key)
        {
            if (_occurrencesDictionary.ContainsKey(_key))
            {
                _occurrencesDictionary.TryGetValue(_key, out int currentValue);
                _occurrencesDictionary[_key] = currentValue + 1;
            }
            else
            {
                _occurrencesDictionary.Add(_key, 1);
            }
        }

        static IEnumerable<string> Split(string _str, int _chunkSize)
        {
            return Enumerable.Range(0, _str.Length / _chunkSize)
                .Select(i => _str.Substring(i * _chunkSize, _chunkSize));
        }

        static string StringToBinary(string _data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in _data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        static void AddWithExists()
        {

        }
    }
}
