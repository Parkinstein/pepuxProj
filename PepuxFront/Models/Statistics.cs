using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PepuxFront.IpServiceLink;

namespace PepuxFront.Models
{
    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class VMRstats_response
    {
        [DataMember(Name = "meta")]
        public meta metas { get; set; }
        [DataMember(Name = "objects")]
        public List<VMRstats> obj { get; set; }
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class meta
    {
        [DataMember(Name = "limit")]
        public int limit { get; set; }
        [DataMember(Name = "next")]
        public object next { get; set; }
        [DataMember(Name = "offset")]
        public int offset { get; set; }
        [DataMember(Name = "previous")]
        public object previous { get; set; }
        [DataMember(Name = "total_count")]
        public int total_count { get; set; }
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public partial class VMRstats
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "start_time")]
        public string start_time { get; set; }
        public string start_time2 { get; set; }
        [DataMember(Name = "end_time")]
        public string end_time { get; set; }
        public string end_time2 { get; set; }
        [DataMember(Name = "duration")]
        public string duration { get; set; }
        [DataMember(Name = "participant_count")]
        public int participant_count { get; set; }
        [DataMember(Name = "participants")]
        public List<Participantstats_uris> obj { get; set; }
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class Participantstats_uris
    {
        public string participants_uri { get; set; }
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class Participantstats_response
    {
        [DataMember(Name = "meta")]
        public meta metas { get; set; }
        [DataMember(Name = "objects")]
        public List<Participantstats> obj { get; set; }
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public partial class Participantstats
    {
        [DataMember(Name = "name")]
        public int name { get; set; }
        [DataMember(Name = "role")]
        public string role { get; set; }
        [DataMember(Name = "local_alias")]
        public string local_alias { get; set; }
        [DataMember(Name = "remote_alias")]
        public string remote_alias { get; set; }
        [DataMember(Name = "start_time")]
        public string start_time { get; set; }
        public string start_time2 { get; set; }
        [DataMember(Name = "end_time")]
        public string end_time { get; set; }
        public string end_time2 { get; set; }
        [DataMember(Name = "duration")]
        public string duration { get; set; }
        [DataMember(Name = "display_name")]
        public string display_name { get; set; }
        [DataMember(Name = "protocol")]
        public string protocol { get; set; }
        [DataMember(Name = "vendor")]
        public string vendor { get; set; }
        [DataMember(Name = "remote_address")]
        public string remote_address { get; set; }
        [DataMember(Name = "remote_port")]
        public string remote_port { get; set; }
        [DataMember(Name = "disconnect_reason")]
        public string disconnect_reason { get; set; }
        [DataMember(Name = "videoCodec_t")]
        public string videoCodec_t { get; set; }
        [DataMember(Name = "media_streams")]
        public List<Mediastreams> obj { get; set; }
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public partial class Mediastreams
    {
        [DataMember(Name = "stream_type")]
        public string stream_type { get; set; }
        [DataMember(Name = "start_time")]
        public string start_time { get; set; }
        public string start_time2 { get; set; }
        [DataMember(Name = "end_time")]
        public string end_time { get; set; }
        public string end_time2 { get; set; }

        [DataMember(Name = "tx_bitrate")]
        public string tx_bitrate { get; set; }
        [DataMember(Name = "tx_codec")]
        public string tx_codec { get; set; }
        [DataMember(Name = "tx_resolution")]
        public string tx_resolution { get; set; }

        [DataMember(Name = "rx_bitrate")]
        public string rx_bitrate { get; set; }
        [DataMember(Name = "rx_codec")]
        public string rx_codec { get; set; }
        [DataMember(Name = "rx_resolution")]
        public string rx_resolution { get; set; }
    }
}