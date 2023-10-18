using machina2.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace machina2.service
{
    public static class CognitiveService
    {

        // CLEF D'API
        // URL (ENDPOINT)
        // private static readonly string API_KEY = "abcdddd";
        // private static readonly string ENDPOINT_URL = "adressenet";


        public static async Task<FaceDetectResult> FaceDetect(Stream imageStream)
        {
            if (imageStream == null)
            {
                return null;
            }

            // var url = ENDPOINT_URL + "detect" + "?returnFaceAttributes=age,gender";
            var url = "http://codeavecjonathan.pythonanywhere.com/face";

            // Header
            // Body

            using (var webClient = new WebClient())
            {

                try
                {

                    webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
                    // webClient.Headers.Add("Ocp-Apim-Subscription-Key") = API_KEY ;
                    // Ocp-Apim-Subscription-Key

                    var data = ReadStream(imageStream);

                    var result = await Task.Run(()=> webClient.UploadData(url, data) );

                    // var result = webClient.UploadData(url, data);

                    if (result == null)
                    {
                        return null;
                    }

                    string Json = Encoding.UTF8.GetString(result, 0, result.Length);

                    var faceResult = Newtonsoft.Json.JsonConvert.DeserializeObject<FaceDetectResult[]>(Json);

                    if (faceResult.Length >= 1)
                    {
                        return faceResult[0];
                    }

                    Console.WriteLine("Réponse OK : " + Json);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception webclient : " + ex.Message);

                }

                return null;

            }


        }

        private static byte[] ReadStream(Stream input)
        {
            BinaryReader b = new BinaryReader(input);
            byte[] data = b.ReadBytes((int)input.Length);
            return data;
        }
    }
}
