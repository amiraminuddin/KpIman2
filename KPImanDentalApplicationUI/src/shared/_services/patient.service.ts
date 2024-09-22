import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { PatientDto, PatientTreamentFormDto } from "../model/AppModel";

@Injectable({
  providedIn: 'root'
})

export class PatientService {

  constructor(
    private http: HttpClient
  ) { }

  private apiUrl = environment.apiUrl + 'Patient/';

  getAllPatients(): Observable<PatientDto[]> {
    return this.http.get<PatientDto[]>(this.apiUrl + 'GetAllPatients').pipe();
  }

  getPatientTreatmentFormById(treatmentId : number): Observable<PatientTreamentFormDto> {
    return this.http.get<PatientTreamentFormDto>(this.apiUrl + 'GetPatientTreatmentFormById?TreatmentId=' + treatmentId ).pipe();
  }


}
