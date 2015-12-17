using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Dicom;
using Newtonsoft.Json;
using Tecknow.MediScan.Entities;

namespace Tecknow.MediScan.UI.Web.Controllers
{
    public class QueryController : BaseAPIController
    {
        #region PrivateEnum

        private enum Checker
        {
            Study,
            Series,
            Instance
        }

        #endregion

        #region Private Constants

        private const string PersonNameTagValue = "00100010";
        private const string PersonIdTagValue = "00100020";
        private const string ModalityTagValue = "00080060";
        private const string NoOfImagesTagValue = "00201208";
        private const string StudyDateTagValue = "00080020";
        private const string StudyNameTagValue = "00081030";
        private const string StudyUidTagValue = "0020000D";
        private const string SeriesNameTagValue = "0008103E";
        private const string SeriesUidTagValue = "0020000E";
        private const string InstanceUidTagValue = "00080018";


        private static string _patientNameParam;
        private static string _patientNamekey;
        private static string _date;

        #endregion

        #region WebApi's

        [System.Web.Http.Route("api/studies")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Study> GetStudyDetails(HttpRequestMessage requestMessage)
        {
            if (requestMessage != null)
            {
                var queryParameters = HttpUtility.ParseQueryString(requestMessage.RequestUri.Query);

                _patientNameParam = queryParameters.AllKeys[0];
                _patientNamekey = queryParameters[_patientNameParam];
                _date = queryParameters["StudyDate"];
            }

            var jsonResponse = GetResponseFromQidoService(_patientNameParam, _patientNamekey, _date, Checker.Study, null,
                null);


            var listOfStudy = new List<Study>();

            foreach (var dictonaryValue in jsonResponse)
            {
                var jsonSeriesResponse = GetResponseFromQidoService(null, null, null, Checker.Series,
                    GetValue(dictonaryValue[StudyUidTagValue]), null);
                var study = new Study {Series = new List<Series>()};

                foreach (var seriesDictionaryValue in jsonSeriesResponse)
                {
                    var series = new Series
                    {
                        SeriesName = GetValue(seriesDictionaryValue[SeriesNameTagValue]),
                        SeriesUid = GetValue(seriesDictionaryValue[SeriesUidTagValue]),
                        StudyUid = GetValue(seriesDictionaryValue[StudyUidTagValue])
                    };

                    study.Series.Add(series);
                }

                study.StudyUid = GetValue(dictonaryValue[StudyUidTagValue]);
                study.PersonName = GetName(dictonaryValue[PersonNameTagValue]);
                study.PersonId = GetValue(dictonaryValue[PersonIdTagValue]);
                study.StudyName = GetValue(dictonaryValue[StudyNameTagValue]);

                study.StudyDate = GetValue(dictonaryValue[StudyDateTagValue]);
                study.Modality = GetValue(dictonaryValue[ModalityTagValue]);
                study.NoOfImages = Convert.ToInt32(GetValue(dictonaryValue[NoOfImagesTagValue]));
                listOfStudy.Add(study);
            }

            return listOfStudy;
        }


        [System.Web.Http.Route("api/{studyUid}/{seriesUid}/instance")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Instance> GetInstanceDetails(string studyUid, string seriesUid)
        {
            var jsonResponse = GetResponseFromQidoService(null, null, null, Checker.Instance, studyUid, seriesUid);
            var listOfInstance = jsonResponse.Select(dictionoryValue => new Instance
            {
                InstanceUid = GetValue(dictionoryValue[InstanceUidTagValue]),
                SeriesUid = GetValue(dictionoryValue[SeriesUidTagValue]),
                StudyUid = GetValue(dictionoryValue[StudyUidTagValue]),
                SeriesName = GetValue(dictionoryValue[SeriesNameTagValue]),
                Modality = GetValue(dictionoryValue[ModalityTagValue]),
                StudyName = GetValue(dictionoryValue[StudyNameTagValue]),
                PersonName = GetName(dictionoryValue[PersonNameTagValue])
            }).ToList();

            return listOfInstance;
        }


        [System.Web.Http.Route("api/{annotation}/{instanceUid}/annotation")]
        [HttpPost]
        public void AddAnnotation(string annotation, string instanceUid)
        {
            var fileName = GetResponseFromQidoServiceForAnnotation(instanceUid);
            var dicomFile = DicomFile.Open(fileName);
            var id = 0;

            var annotatationIdSeperator = new[] {"[0028]"};

            var burnedInAnnotation = dicomFile.Dataset.Get<string>(DicomTag.BurnedInAnnotation);

            if (burnedInAnnotation != null)
            {
                var annotationId = burnedInAnnotation.Split(annotatationIdSeperator, StringSplitOptions.None);
                var totalAnnotation = annotationId.Length;

                id = Convert.ToInt32(annotationId[totalAnnotation - 4]) + 1;

                dicomFile.Dataset.Add(DicomTag.BurnedInAnnotation,
                    burnedInAnnotation + "[0028]" + id + "[0028]" + annotation + "[0028]" +
                    DateTime.Now.ToString("dd-MM-yyyy") + "[0028]" + DateTime.Now.ToString("HH:MM"));
                var newFileName = fileName.Substring(0, 20) + "-" + id + ".dcm";
                dicomFile.Save(newFileName);
            }
            else
            {
                dicomFile.Dataset.Add(DicomTag.BurnedInAnnotation,
                    "[0028]0[0028]" + annotation + "[0028]" + DateTime.Now.ToString("dd-MM-yyyy") + "[0028]" +
                    DateTime.Now.ToString("HH:MM"));
                var newFileName = fileName.Substring(0, 20) + "-0.dcm";
                dicomFile.Save(newFileName);
            }
            File.Delete(fileName);


            var baseUrl = ConfigurationManager.AppSettings["Annotation"];
            var webRequest = WebRequest.Create(baseUrl + "/" + id + "/" + instanceUid + "/update");
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";

            webRequest.GetResponse();
        }


        [System.Web.Http.Route("api/{instanceUid}/annotation")]
        [System.Web.Http.HttpGet]
        public Annotation GetAnnotation(string instanceUid)
        {
            var annotation = new Annotation
            {
                AnnotationComments = new List<string>(),
                CommentDate = new List<string>(),
                CommentTime = new List<string>()
            };


            var fileName = GetResponseFromQidoServiceForAnnotation(instanceUid);
            var dicomFile = DicomFile.Open(fileName);

            var annotatationSeperator = new[] {"[0028]"};

            var burnedInAnnotation = dicomFile.Dataset.Get<string>(DicomTag.BurnedInAnnotation);

            if (burnedInAnnotation != null)
            {
                var spilitBurnedInAnnotation = burnedInAnnotation.Split(annotatationSeperator, StringSplitOptions.None);
                var totalBuredInAnnotaion = spilitBurnedInAnnotation.Length;


                for (var i = 0; i < (totalBuredInAnnotaion - 1)/4; i++)
                {
                    annotation.AnnotationComments.Add(spilitBurnedInAnnotation[(4*i) + 2]);
                    annotation.CommentDate.Add(spilitBurnedInAnnotation[(4*i) + 3]);
                    annotation.CommentTime.Add(spilitBurnedInAnnotation[(4*i) + 4]);
                }
            }
            return annotation;
        }

        #endregion

        #region PrivateMethods

        private string GetResponseFromQidoServiceForAnnotation(string instanceUid)
        {
            string responseString = null;

            var baseUrl = ConfigurationManager.AppSettings["Annotation"];

            var webRequest =
                WebRequest.Create(baseUrl + "/" + instanceUid + "/getFileName") as HttpWebRequest;

            if (webRequest == null) return null;
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";

            var webResponse = webRequest.GetResponse() as HttpWebResponse;
            if (webResponse != null)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var fullContent = streamReader.ReadToEnd();
                    responseString =
                        JsonConvert.DeserializeObject<string>(fullContent);
                }
            }

            return responseString;
        }


        private IEnumerable<Dictionary<string, object>> GetResponseFromQidoService(string parameter, string key,
            string date,
            Checker checker, string studyUid, string seriesUid)
        {
            HttpWebRequest webRequest = null;

            var baseUrl = ConfigurationManager.AppSettings["QIDOService"];

            #region SwitchCase

            switch (checker)
            {
                case Checker.Study:
                {
                    if (date == null)
                    {
                        webRequest = WebRequest.Create(baseUrl + "?" + parameter + "=" + key)
                            as HttpWebRequest;
                    }
                    else
                    {
                        webRequest = WebRequest.Create(baseUrl + "?" + parameter + "=" + key + "&StudyDate=" + date)
                            as HttpWebRequest;
                    }
                }
                    break;
                case Checker.Series:
                {
                    webRequest =
                        WebRequest.Create(baseUrl + studyUid + "/series") as
                            HttpWebRequest;
                }
                    break;
                case Checker.Instance:
                {
                    webRequest =
                        WebRequest.Create(baseUrl + studyUid + "/series/" + seriesUid +
                                          "/instances") as HttpWebRequest;
                }
                    break;
            }

            #endregion

            if (webRequest == null) return null;
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";


            var webResponse = webRequest.GetResponse() as HttpWebResponse;

            //if (webResponse == null) return responseDictionory;

            IEnumerable<Dictionary<string, object>> responseDictionory;
            if (webResponse == null) return null;
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                var fullContent = streamReader.ReadToEnd();
                responseDictionory =
                    JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, object>>>(fullContent);
            }
            return responseDictionory;
        }

        private static string GetValue(dynamic p)
        {
            return p.Value[0].ToString();
        }

        private static string GetName(dynamic p)
        {
            return p.Value[0].Alphabetic.Value.ToString();
        }

        #endregion
    }
}