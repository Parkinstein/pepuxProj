/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.__recurring_template='<div class="dhx_form_repeat"> <form> <div class="dhx_repeat_left"> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="day" />Quotidiano</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="week"/>Settimanale</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="month" checked />Mensile</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="year" />Annuale</label> </div> <div class="dhx_repeat_divider"></div> <div class="dhx_repeat_center"> <div style="display:none;" id="dhx_repeat_day"> <label><input class="dhx_repeat_radio" type="radio" name="day_type" value="d"/>Ogni</label><input class="dhx_repeat_text" type="text" name="day_count" value="1" />giorno<br /> <label><input class="dhx_repeat_radio" type="radio" name="day_type" checked value="w"/>Ogni giornata lavorativa</label> </div> <div style="display:none;" id="dhx_repeat_week"> Ripetere ogni<input class="dhx_repeat_text" type="text" name="week_count" value="1" />settimana:<br /> <table class="dhx_repeat_days"> <tr> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="1" />Lunedì</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="4" />Jovedì</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="2" />Martedì</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="5" />Venerdì</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="3" />Mercoledì</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="6" />Sabato</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="0" />Domenica</label><br /><br /> </td> </tr> </table> </div> <div id="dhx_repeat_month"> <label><input class="dhx_repeat_radio" type="radio" name="month_type" value="d"/>Ripetere</label><input class="dhx_repeat_text" type="text" name="month_day" value="1" />giorno ogni<input class="dhx_repeat_text" type="text" name="month_count" value="1" />mese<br /> <label><input class="dhx_repeat_radio" type="radio" name="month_type" checked value="w"/>Il</label><input class="dhx_repeat_text" type="text" name="month_week2" value="1" /><select name="month_day2"><option value="1" selected >Lunedì<option value="2">Martedì<option value="3">Mercoledì<option value="4">Jovedì<option value="5">Venerdì<option value="6">Sabato<option value="0">Domenica</select>ogni<input class="dhx_repeat_text" type="text" name="month_count2" value="1" />mese<br /> </div> <div style="display:none;" id="dhx_repeat_year"> <label><input class="dhx_repeat_radio" type="radio" name="year_type" value="d"/>Ogni</label><input class="dhx_repeat_text" type="text" name="year_day" value="1" />giorno<select name="year_month"><option value="0" selected >Gennaio<option value="1">Febbraio<option value="2">Marzo<option value="3">Aprile<option value="4">Maggio<option value="5">Jiugno<option value="6">Luglio<option value="7">Agosto<option value="8">Settembre<option value="9">Ottobre<option value="10">Novembre<option value="11">Dicembre</select>mese<br /> <label><input class="dhx_repeat_radio" type="radio" name="year_type" checked value="w"/>Il</label><input class="dhx_repeat_text" type="text" name="year_week2" value="1" /><select name="year_day2"><option value="1" selected >Lunedì<option value="2">Martedì<option value="3">Mercoledì<option value="4">Jovedì<option value="5">Venerdì<option value="6">Sabato<option value="0">Domenica</select>del<select name="year_month2"><option value="0" selected >Gennaio<option value="1">Febbraio<option value="2">Marzo<option value="3">Aprile<option value="4">Maggio<option value="5">Jiugno<option value="6">Luglio<option value="7">Agosto<option value="8">Settembre<option value="9">Ottobre<option value="10">Novembre<option value="11">Dicembre</select><br /> </div> </div> <div class="dhx_repeat_divider"></div> <div class="dhx_repeat_right"> <label><input class="dhx_repeat_radio" type="radio" name="end" checked/>Senza data finale</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="end" />Dopo</label><input class="dhx_repeat_text" type="text" name="occurences_count" value="1" />occorenze<br /> <label><input class="dhx_repeat_radio" type="radio" name="end" />Fine</label><input class="dhx_repeat_date" type="text" name="date_of_end" value="'+e.config.repeat_date_of_end+'" /><br /> </div> </form> </div> <div style="clear:both"> </div>';

});