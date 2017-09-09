using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using Microsoft.ProjectOxford.Emotion.Contract;
using System.Linq;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;

namespace LiveCameraForm
{
    internal class Aggregation
    {
        public static Tuple<string, float> GetDominantEmotion(Microsoft.ProjectOxford.Common.Contract.EmotionScores scores)
        {
            var Item = scores.ToRankedList().OrderByDescending(o => o.Value).Select(kv => new Tuple<string, float>(kv.Key, kv.Value)).FirstOrDefault();
            if (Item != null && Item.Item1 == "Neutral")
            {
                var score2 = scores.ToRankedList().OrderByDescending(o => o.Value).Take(2).Select(kv => new Tuple<string, float>(kv.Key, kv.Value)).ToList();
                if (score2 != null )
                {
                    var nonWinner = score2.OrderBy(o => o.Item2).FirstOrDefault();
                    var Winner = score2.OrderByDescending(o => o.Item2).FirstOrDefault();
                    
                    if (Winner.Item2 - nonWinner.Item2 < 0.9 && Winner.Item2 < 0.7)
                    {
                        return nonWinner;
                    }
                }
            }


            return scores.ToRankedList().OrderByDescending(o => o.Value).Select(kv => new Tuple<string, float>(kv.Key, kv.Value)).First();
        }

        public static string SummarizeEmotion(Microsoft.ProjectOxford.Common.Contract.EmotionScores scores)
        {
            var bestEmotion = Aggregation.GetDominantEmotion(scores);




            return string.Format("{0}: {1:N1}", bestEmotion.Item1, bestEmotion.Item2);
        }

        public static string SummarizeFaceAttributes(FaceAttributes attr)
        {
            List<string> attrs = new List<string>();
            if (attr.Gender != null) attrs.Add(attr.Gender);
            if (attr.Age > 0) attrs.Add(attr.Age.ToString());
            if (attr.HeadPose != null)
            {
                // Simple rule to estimate whether person is facing camera. 
                bool facing = Math.Abs(attr.HeadPose.Yaw) < 25;
                attrs.Add(facing ? "facing camera" : "not facing camera");
            }
            return string.Join(", ", attrs);
        }


    }
}
