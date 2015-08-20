/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.__recurring_template='<div class="dhx_form_repeat"> <form> <div class="dhx_repeat_left"> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="day" />Daglig</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="week"/>Ugenlig</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="month" checked />Månedlig</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="year" />Årlig</label> </div> <div class="dhx_repeat_divider"></div> <div class="dhx_repeat_center"> <div style="display:none;" id="dhx_repeat_day"> <label>Gentager sig:<br/></label> <label><input class="dhx_repeat_radio" type="radio" name="day_type" value="d"/>Hver</label><input class="dhx_repeat_text" type="text" name="day_count" value="1" />dag<br /> <label><input class="dhx_repeat_radio" type="radio" name="day_type" checked value="w"/>På hver arbejdsdag</label> </div> <div style="display:none;" id="dhx_repeat_week"> Gentager sig hver<input class="dhx_repeat_text" type="text" name="week_count" value="1" />uge på følgende dage:<br /> <table class="dhx_repeat_days"> <tr> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="1" />Mandag</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="4" />Torsdag</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="2" />Tirsdag</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="5" />Fredag</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="3" />Onsdag</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="6" />Lørdag</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="0" />Søndag</label><br /><br /> </td> </tr> </table> </div> <div id="dhx_repeat_month"> <label>Gentager sig:<br/></label> <label><input class="dhx_repeat_radio" type="radio" name="month_type" value="d"/>Hver den</label><input class="dhx_repeat_text" type="text" name="month_day" value="1" /> i hver<input class="dhx_repeat_text" type="text" name="month_count" value="1" />måned<br /> <label><input class="dhx_repeat_radio" type="radio" name="month_type" checked value="w"/>Den</label><input class="dhx_repeat_text" type="text" name="month_week2" value="1" /><select name="month_day2"><option value="1" selected >Mandag<option value="2">Tirsdag<option value="3">Onsdag<option value="4">Torsdag<option value="5">Fredag<option value="6">Lørdag<option value="0">Søndag</select>hver<input class="dhx_repeat_text" type="text" name="month_count2" value="1" />måned<br /> </div> <div style="display:none;" id="dhx_repeat_year"> <label>Gentager sig:</label> <label><input class="dhx_repeat_radio" type="radio" name="year_type" value="d"/>På hver</label><input class="dhx_repeat_text" type="text" name="year_day" value="1" />dag i<select name="year_month"><option value="0" selected >Januar<option value="1">Februar<option value="2">März<option value="3">April<option value="4">Mai<option value="5">Juni<option value="6">Juli<option value="7">August<option value="8">September<option value="9">Oktober<option value="10">November<option value="11">Dezember</select><br /> <label><input class="dhx_repeat_radio" type="radio" name="year_type" checked value="w"/>Den</label><input class="dhx_repeat_text" type="text" name="year_week2" value="1" /><select name="year_day2"><option value="1" selected >Mandag<option value="2">Tirsdag<option value="3">Onsdag<option value="4">Torsdag<option value="5">Fredag<option value="6">Lørdag<option value="0">Søndag</select>i<select name="year_month2"><option value="0" selected >Januar<option value="1">Februar<option value="2">März<option value="3">April<option value="4">Mai<option value="5">Juni<option value="6">Juli<option value="7">August<option value="8">September<option value="9">Oktober<option value="10">November<option value="11">Dezember</select><br /> </div> </div> <div class="dhx_repeat_divider"></div> <div class="dhx_repeat_right"> <label><input class="dhx_repeat_radio" type="radio" name="end" checked/>Ingen slutdato</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="end" />Efter</label><input class="dhx_repeat_text" type="text" name="occurences_count" value="1" />gentagelse<br /> <label><input class="dhx_repeat_radio" type="radio" name="end" />Slut</label><input class="dhx_repeat_date" type="text" name="date_of_end" value="'+e.config.repeat_date_of_end+'" /><br /> </div> </form> </div> <div style="clear:both"> </div>';

});