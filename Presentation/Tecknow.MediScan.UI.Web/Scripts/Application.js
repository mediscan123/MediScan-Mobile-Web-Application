
var imageArray = [];
var instanceUid = [];
var seriesUid = [];
var currentImageIndex = 0;
var CurrentImageUid = 0;

var annotationArray = [];
var element = document.getElementById("maindiv");
var showcomments;
var tempValue = 1;


function ViewModel() {
    var self = this;

    self.queryparams = ko.observableArray(["PatientName", "Modality", "PatientID"]);
    self.queryKey = ko.observable();
    self.selectedvalue = ko.observable();

    self.fromdate = ko.observable();
    self.todate = ko.observable();

    self.favoriteurl = ko.observable("#favorite");


    self.PersonData = ko.observable();
    self.Person = ko.observableArray([]);

    self.details = ko.observable();

    self.tabcontents = ko.observableArray();
    self.allinstances = ko.observableArray([]);


    self.PostComment = function () {

        var index = instanceUid.indexOf(CurrentImageUid);
        var annotation = self.tabcontents()[index].annotations;

        $.post("/api/" + annotation + "/" + CurrentImageUid + "/annotation", function () {

        });

        $("#latestdiv").append("  <div class=\"media downunderline\">" +
           " <div class=\"media-left media-middle\"> " +
                 "<a href=\"#\"><img  src=\"/Images/fr-01.jpg\" alt=\"...\" class=\"img-circle imagediv\"></a>" +
            "</div> " +
             " <div class=\"media-body\"> " +
                "<h4 class=\"media-heading\"><span style=\"font-size: 22px\">Renga - </span><span>March 23 2011 , </span><span>11:36 AM </span> </h4> " +
                " <span> " + annotation + "</span>" +
        " </div> " +
    " </div>");
    }

    self.search = function () {
        if (self.queryKey() !== undefined) {
            location.hash = "#study/" + self.selectedvalue() + "/ " + self.queryKey();
        } else {
            toastr.error("Enter the Values");
        }
    };

    self.InstanceSearch = function (uid) {
        if (tempValue === 1) {
            $("#tabs").append("<ul id=\"myTab\" class=\"nav nav-tabs\"></ul>");
            $("#myTab").append("<li class=\"active\" ><a href=\"#" + uid.SeriesName.replace(" ", "") + "\"data-toggle=\"tab\" data-bind=\"click : $parent.InstanceSearch\" > "
                + uid.SeriesName + "</a></li>");
            tempValue = tempValue + 1;
        } else {
            $("#myTab").append("<li><a href=\"#" + uid.SeriesName.replace(" ", "") + "\"data-toggle=\"tab\" data-bind=\"click : $parent.InstanceSearch\" > "
                + uid.SeriesName + "</a></li>");
        }


        seriesUid.push(uid.SeriesUid);

        self.tabcontents.push({
            divid: uid.SeriesName.replace(" ", ""),
            PatientName: "Patient Name - " + uid.PersonName,
            SeriesName: "Series Name - " + uid.SeriesName,
            StudyName: "Study Name" + uid.StudyName,
            Modality: "Modality" + uid.Modality,
            annotations: "",
            noofcomments: "",
            commentindivID: "commentindiv" + uid.SeriesUid.replace(/\./g, ''),
            carosalDivId: "CarosalDiv" + uid.SeriesUid.replace(/\./g, ''),
            dicomImageId: "dicomImage" + uid.SeriesUid.replace(/\./g, ''),
            mainDicomImageId: "mainDicomImageId" + uid.SeriesUid.replace(/\./g, ''),
            PostComment: function () { }

        });

        toastr.success(self.tabcontents()[0].mainDicomImageId);

        location.hash = "#/instance/" + uid.StudyUid + "/" + uid.SeriesUid;

    };

    function activate(id) {
        $('a').removeClass('active');
        $(id).addClass('active');
    }

    function disableAllTools() {
        cornerstoneTools.wwwc.disable(element);
        
        cornerstoneTools.zoom.activate(element, 4); // 4 is right mouse button
        cornerstoneTools.probe.deactivate(element, 1);
        cornerstoneTools.length.deactivate(element, 1);
        cornerstoneTools.ellipticalRoi.deactivate(element, 1);
        
    }

    self.ViewImage = function (obj) {

        var index = instanceUid.indexOf(obj.instanceid);

        CurrentImageUid = obj.instanceid;

        element = document.getElementById("dicomImage" + obj.corosalseriesuid.replace(/\./g, ''));
        cornerstone.enable(element);
        cornerstone.loadAndCacheImage(imageArray[index]).then(function (image) {
            cornerstone.displayImage(element, image);
            cornerstoneTools.mouseInput.enable(element);
           
            cornerstoneTools.wwwc.activate(element, 1); // ww/wc is the default tool for left mouse button
            cornerstoneTools.pan.activate(element, 2); // pan is the default tool for middle mouse button
             // zoom is the default tool for right mouse button
            cornerstoneTools.zoomWheel.activate(element); // zoom is the default tool for middle mouse wheel
          

        });

        $("#latestdiv").empty();

        $("#commentindiv" + obj.corosalseriesuid.replace(/\./g, '')).empty();

        $.get("api/" + obj.instanceid + "/annotation", function (data) {
            var length = data.AnnotationComments.length;
            document.getElementById("noofcomments").innerHTML = length + "- Comments";
            for (var i = length - 1; i >= 0; i--) {
                $("#commentindiv" + obj.corosalseriesuid.replace(/\./g, '')).append("  <div class=\"media downunderline\">" +
           " <div class=\"media-left media-middle\"> " +
                 "<a href=\"#\"><img  src=\"/Images/fr-01.jpg\" alt=\"...\" class=\"img-circle imagediv\"></a>" +
            "</div> " +
             " <div class=\"media-body\"> " +
                "<h4 class=\"media-heading\"><span style=\"font-size: 22px\">Renga -  </span><span>" + data.CommentDate[i] + " , </span><span> " + data.CommentTime[i] + " </span> </h4> " +
                " <span>" + data.AnnotationComments[i] + "</span>" +
               " </div> " +
               " </div>");

                if ((length - i) === 4) {
                    $("#commentindiv" + obj.corosalseriesuid.replace(/\./g, '')).append("</br><center><button class=\"btn btn-primary btn-sm\" data-toggle=\"modal\" data-target=\"#" + obj.corosalseriesuid.replace(/\./g, '') + "\"> View  " + length + " More Comments  </button></center></br>");
                    $("#commentindiv" + obj.corosalseriesuid.replace(/\./g, '')).append(" <div class=\"modal fade\" id=\"" + obj.corosalseriesuid.replace(/\./g, '') + "\" tabindex=\"-1\" role=\"dialog\" " +
                    "aria-labelledby=\"myModalLabel\" aria-hidden=\"true\">" +
                    "<div class=\"modal-dialog\"> " +
                         "<div class=\"modal-content\">" +
                                "<div  id= \"modelbody" + obj.corosalseriesuid.replace(/\./g, '') + "\"  class=\"modal-body\">" +
                                "</div>" +
                     "  </div>" +
                    "</div>" +
                     " </div>  ");
                    break;
                }
            }


            for (var j = length - 1; j >= 0; j--) {

                $("#modelbody" + obj.corosalseriesuid.replace(/\./g, '')).append(" <div class=\"media downunderline\"> "+
                    "<div class=\"media-left media-middle\">" +
                        "<a href=\"#\"><img src=\"/Images/fr-01.jpg\" alt=\"...\" class=\"img-circle imagediv\"></a>" +
                    "</div> "+
                "<div class=\"media-body\">"+
                        "<h4 class=\"media-heading\"><span style=\"font-size: 22px\">Renga - </span><span> " + data.CommentDate[j] + " , </span><span> " + data.CommentTime[j] + " </span> </h4>" +
                        "<span>" + data.AnnotationComments[j] + "</span> " +
                " </div> " +
            " </div> " );

            }

        });
        

    };


    function updateImage(element, id) {
        var index = instanceUid.indexOf(id);
        cornerstone.enable(element);
        cornerstone.loadAndCacheImage(imageArray[index]).then(function (image) {
            cornerstone.displayImage(element, image);
        });
    }


    function createDiv(seriesuid, len) {
        var element1 = document.getElementById("CarosalDiv" + seriesuid.replace(/\./g, ''));
        for (var i = 0; i < len; i++) {

            var child = element1.children[(imageArray.length - len) + i];
            updateImage(child, child.id);
        }
        for (var j = 0; j < imageArray.length - len; j++) {
            $("#" + element1.children[j].id).remove();
        }
    }

    Sammy(function () {

        this.get("#favorite", function() {
            alert("hi");
        });

        this.get("#study/:param/:key", function () {

            if (self.fromdate() || self.todate() == undefined) {
                $.get("/api/studies/?" + self.selectedvalue() + "=" + self.queryKey(),
                    function (data) {

                        if (data.length === 0) {
                            toastr.warning("Search Result For " + self.selectedvalue() + ":" + self.queryKey() + " Not Found. Refine your search");
                            self.PersonData(null);
                        }

                        var simuatedresult = [];

                        for (var i = 0; i < data.length; i++) {
                            simuatedresult.push(data[i]);


                            self.PersonData({ Person: simuatedresult });
                        }
                    });
            } else {
                $.get("/api/studies/?" + self.selectedvalue() + "=" + self.queryKey() + "&StudyDate=" + self.fromdate().replace(/-|\//g, "") + "-" + self.todate().replace(/-|\//g, ""),
                    function (data) {

                        if (data.length === 0) {
                            toastr.warning("Search Result For " + self.selectedvalue() + ":" + self.queryKey() + " Not Found. Refine your search");
                            self.PersonData(null);
                        }

                        var simuatedresult = [];
                        var seriesArray = [];
                        for (var i = 0; i < data.length; i++) {
                            simuatedresult.push(data[i]);
                            seriesArray.push(data[i].SeriesName);
                            seriesArray.push(data[i].SeriesUid);
                            simuatedresult.push(seriesArray);
                            self.PersonData({ Person: simuatedresult });
                        }
                    });
            }


        });

        this.get("#/:instance/:studyuid/:seriesuid", function () {


            $.get("api/" + this.params.studyuid + "/" + this.params.seriesuid + "/instance", function (data) {
                if (data.length === 0) {
                    toastr.warning("Series for this particular study not found");
                    self.SeriesData(null);
                } else {
                    for (var i = 0; i < data.length; i++) {



                        var imageId = "http://localhost:8042/wado?requestType=WADO&studyUID=" + data[i].StudyUid + "&seriesUID=" +
                            data[i].SeriesUid + "&objectUID=" + data[i].InstanceUid + "&contentType=application%2Fdicom&transferSyntax=1.2.840.10008.1.2.4.90";

                        imageId = "dicomweb:" + imageId;

                        self.allinstances.push({ corosalseriesuid: data[0].SeriesUid, instanceid: data[i].InstanceUid, ImageId: imageId });

                        imageArray.push(imageId);
                        instanceUid.push(data[i].InstanceUid);

                    }
                }
                createDiv(data[0].SeriesUid, data.length);
            });
        });
    }).run();
};


ko.applyBindings(new ViewModel());

