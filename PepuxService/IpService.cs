﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Services.Description;
using Newtonsoft.Json;

namespace PepuxService
{
    [ServiceContract]
    public interface IPService
    {

        [OperationContract]
        List<ActiveConfs> GetActiveConfs();
        [OperationContract]
        List<Service> GetDataLocal();
        [OperationContract]
        List<Participants> GetActiveParts(string confname);
        [OperationContract]
        List<AllVmrs> GetVmrList();
        [OperationContract]
        string GetToken(string confname, string dispname,string pin);

    }

    #region UsersClasses

    [DataContract]
    public class ADUsers
    {

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string Group { get; set; }
        

    }

   [DataContract(Name = "locusers")]
    public class LocalUsers
    {

        [DataMember]
        public string AdName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Role { get; set; }
  }

    #endregion

    #region AllActiveConfsClasses

    [DataContract(Name = "")]
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseParent
    {
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public Meta metas { get; set; }

        [DataMember(Order = 2), Newtonsoft.Json.JsonProperty("objects")]

        public List<ActiveConfs> obj { get; set; }
    }

    [DataContract(Name = "meta")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Meta
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
    public partial class ActiveConfs
    {
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public string id { get; set; }

        [DataMember(Order = 2), Newtonsoft.Json.JsonProperty]
        public bool is_locked { get; set; }

        [DataMember(Order = 3), Newtonsoft.Json.JsonProperty]
        public string name { get; set; }

        [DataMember(Order = 4), Newtonsoft.Json.JsonProperty]
        public string resource_uri { get; set; }

        [DataMember(Order = 5), Newtonsoft.Json.JsonProperty]
        public string service_type { get; set; }

        [DataMember(Order = 6), Newtonsoft.Json.JsonProperty]
        public string start_time { get; set; }
        [DataMember(Order = 7), Newtonsoft.Json.JsonProperty]
        public string start_time2 { get; set; }

        [DataMember(Order = 8), Newtonsoft.Json.JsonProperty]
        public string tag { get; set; }
        [DataMember(Order = 9), Newtonsoft.Json.JsonProperty]
        public string lock_path { get; set; }

    }

    #endregion

    #region Getting_all_participants_for_a_conference
    [DataContract(Name = "parts")]
    public class RootObject
    {
        [DataMember(Name = "meta")]
        public MetaData metadata { get; set; }
        [DataMember(Order = 2), Newtonsoft.Json.JsonProperty("objects")]
        public List<Participants> participants { get; set; }
    }
    [DataContract(Name = "meta")]
    [JsonObject(MemberSerialization.OptOut)]
    public class MetaData
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
    [DataContract(Name = "participants")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Participants
    {
        [DataMember(Name = "bandwidth")]
        public int bandwidth { get; set; }
        [DataMember(Name = "call_direction")]
        public string call_direction { get; set; }
        [DataMember(Name = "call_uuid")]
        public string call_uuid { get; set; }
        [DataMember(Name = "conference")]
        public string conference { get; set; }
        [DataMember(Name = "connect_time")]
        public string connect_time { get; set; }
        [DataMember(Name = "destination_alias")]
        public string destination_alias { get; set; }
        [DataMember(Name = "display_name")]
        public string display_name { get; set; }
        [DataMember(Name = "encryption")]
        public string encryption { get; set; }
        [DataMember(Name = "has_media")]
        public bool has_media { get; set; }
        [DataMember(Name = "id")]
        public string id { get; set; }
        [DataMember(Name = "is_muted")]
        public bool is_muted { get; set; }
        [DataMember(Name = "is_on_hold")]
        public bool is_on_hold { get; set; }
        [DataMember(Name = "is_presentation_supported")]
        public bool is_presentation_supported { get; set; }
        [DataMember(Name = "is_presenting")]
        public bool is_presenting { get; set; }
        [DataMember(Name = "is_streaming")]
        public bool is_streaming { get; set; }
        [DataMember(Name = "license_count")]
        public int license_count { get; set; }
        [DataMember(Name = "media_node")]
        public string media_node { get; set; }
        [DataMember(Name = "parent_id")]
        public string parent_id { get; set; }
        [DataMember(Name = "participant_alias")]
        public string participant_alias { get; set; }
        [DataMember(Name = "protocol")]
        public string protocol { get; set; }
        [DataMember(Name = "remote_address")]
        public string remote_address { get; set; }
        [DataMember(Name = "remote_port")]
        public int remote_port { get; set; }
        [DataMember(Name = "resource_uri")]
        public string resource_uri { get; set; }
        [DataMember(Name = "role")]
        public string role { get; set; }
        [DataMember(Name = "service_tag")]
        public string service_tag { get; set; }
        [DataMember(Name = "service_type")]
        public string service_type { get; set; }
        [DataMember(Name = "signalling_node")]
        public string signalling_node { get; set; }
        [DataMember(Name = "source_alias")]
        public string source_alias { get; set; }
        [DataMember(Name = "system_location")]
        public string system_location { get; set; }
        [DataMember(Name = "vendor")]
        public string vendor { get; set; }
    }
    #endregion

    #region Getting the media statistics for a participant

    #endregion

    #region AllVMRS

    [DataContract(Name = "allvmr")]
    [JsonObject(MemberSerialization.OptIn)]
    public class VmrParent
    {
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public Meta metas { get; set; }

        [DataMember(Order = 2), Newtonsoft.Json.JsonProperty("objects")]

        public List<AllVmrs> obj { get; set; }
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public partial class AllVmrs
    {
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public List<Aliasess> aliases { get; set; }

        [DataMember(Order = 2), Newtonsoft.Json.JsonProperty]
        public bool allow_guests { get; set; }

        [DataMember(Order = 3), Newtonsoft.Json.JsonProperty]
        public List<AutoPartis> automatic_participants { get; set; }

        [DataMember(Order = 4), Newtonsoft.Json.JsonProperty]
        public string description { get; set; }

        [DataMember(Order = 5), Newtonsoft.Json.JsonProperty]
        public bool force_presenter_into_main { get; set; }

        [DataMember(Order = 6), Newtonsoft.Json.JsonProperty]
        public string guest_pin { get; set; }
        [DataMember(Order = 7), Newtonsoft.Json.JsonProperty]
        public string guest_view { get; set; }

        [DataMember(Order = 8), Newtonsoft.Json.JsonProperty]
        public string host_view { get; set; }
        [DataMember(Order = 9), Newtonsoft.Json.JsonProperty]
        public int id { get; set; }
        [DataMember(Order = 10), Newtonsoft.Json.JsonProperty]

        public string ivr_theme { get; set; }
        [DataMember(Order = 11), Newtonsoft.Json.JsonProperty]
        public string max_callrate_in { get; set; }
        [DataMember(Order = 12), Newtonsoft.Json.JsonProperty]
        public string max_callrate_out { get; set; }
        [DataMember(Order = 13), Newtonsoft.Json.JsonProperty]
        public string name { get; set; }
        [DataMember(Order = 14), Newtonsoft.Json.JsonProperty]
        public string participant_limit { get; set; }
        [DataMember(Order = 15), Newtonsoft.Json.JsonProperty]
        public string pin { get; set; }
        [DataMember(Order = 16), Newtonsoft.Json.JsonProperty]
        public string resource_uri { get; set; }
        [DataMember(Order = 17), Newtonsoft.Json.JsonProperty]
        public string service_type { get; set; }
        [DataMember(Order = 18), Newtonsoft.Json.JsonProperty]
        public string sync_tag { get; set; }
        [DataMember(Order = 19), Newtonsoft.Json.JsonProperty]
        public string tag { get; set; }

    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public partial class Aliasess
    {
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public string alias { get; set; }
        [DataMember(Order = 2), Newtonsoft.Json.JsonProperty]
        public string conference { get; set; }
        [DataMember(Order = 3), Newtonsoft.Json.JsonProperty]
        public string description { get; set; }
        [DataMember(Order = 4), Newtonsoft.Json.JsonProperty]
        public int id { get; set; }
    }
    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public partial class AutoPartis
    {
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public string alias { get; set; }
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public string description { get; set; }
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public string dtmf_sequence { get; set; }
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public int id { get; set; }
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public string protocol { get; set; }
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public string role { get; set; }
        [DataMember(Order = 1), Newtonsoft.Json.JsonProperty]
        public bool streaming { get; set; }
    }
    #endregion




}
