using System;
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
        List<Objects> GetActiveConfs();
        [OperationContract]
        List<Service> GetDataLocal();
        [OperationContract]
        List<Participants> GetActiveParts(string confname);
        [OperationContract]
        Result TokenRequest();

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
    public class ResponseParent
    {
        [DataMember(Name = "meta")]
        public Meta metas { get; set; }

        [DataMember(Name = "objects")]
        public List<Objects> obj { get; set; }
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

    [DataContract(Name = "objects")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Objects
    {
        [DataMember(Name = "id")]
        public string id { get; set; }

        [DataMember(Name = "is_locked")]
        public bool is_locked { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "resource_uri")]
        public string resource_uri { get; set; }

        [DataMember(Name = "service_type")]
        public string service_type { get; set; }

        [DataMember(Name = "start_time")]
        public string start_time { get; set; }
        [DataMember(Name = "start_time2")]
        public string start_time2 { get; set; }
        
        [DataMember(Name = "tag")]
        public string tag { get; set; }
        [DataMember(Name = "lock_path")]
        public string lock_path { get; set; }

    }

    #endregion

    #region Getting_all_participants_for_a_conference
    [DataContract(Name = "")]
    public class RootObject
    {
        [DataMember(Name = "meta")]
        public MetaData metadata { get; set; }
        [DataMember(Name = "participants")]
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
    [DataContract(Name = "participantsAll")]
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

    #region Get all VMR`s
    public class AllWmrs
    {
        public Metas meta { get; set; }
        public List<Vmrs> objects { get; set; }
    }
    public class Metas
    {
        public int limit { get; set; }
        public object next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total_count { get; set; }
    }
    [DataContract(Name = "obj")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Vmrs
    {
        public List<Aliases> aliases { get; set; }
        public bool allow_guests { get; set; }
        public List<object> automatic_participants { get; set; }
        public string description { get; set; }
        public bool force_presenter_into_main { get; set; }
        public string guest_pin { get; set; }
        public object guest_view { get; set; }
        public string host_view { get; set; }
        public int id { get; set; }
        public object ivr_theme { get; set; }
        public object max_callrate_in { get; set; }
        public object max_callrate_out { get; set; }
        public string name { get; set; }
        public object participant_limit { get; set; }
        public string pin { get; set; }
        public string resource_uri { get; set; }
        public string service_type { get; set; }
        public string tag { get; set; }
    }

    public class Aliases
    {
        public string alias { get; set; }
        public string conference { get; set; }
        public string description { get; set; }
        public string id { get; set; }

    }
    #endregion

#region Get token for V2
    [DataContract(Name = "")]
    public class TokenRoot
    {
        public string status { get; set; }
         [DataMember(Name = "tokenres")]
        public Result result { get; set; }
    }
    public class Stun
    {
        public string url { get; set; }
    }

    public class Version
    {
        public string pseudo_version { get; set; }
        public string version_id { get; set; }
    }
    [DataContract(Name = "tokenclass")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Result
    {
        [DataMember(Name = "participant_uuid")]
        public string participant_uuid { get; set; }
        [DataMember(Name = "display_name")]
        public string display_name { get; set; }
        [DataMember(Name = "stun")]
        public List<Stun> stun { get; set; }
        [DataMember(Name = "analytics_enabled")]
        public bool analytics_enabled { get; set; }
        [DataMember(Name = "expires")]
        public string expires { get; set; }
        [DataMember(Name = "token")]
        public string token { get; set; }
        [DataMember(Name = "version")]
        public Version version { get; set; }
        [DataMember(Name = "role")]
        public string role { get; set; }
        [DataMember(Name = "service_type")]
        public string service_type { get; set; }
        [DataMember(Name = "chat_enabled")]
        public bool chat_enabled { get; set; }
        [DataMember(Name = "current_service_type")]
        public string current_service_type { get; set; }
       
    }



#endregion

}
