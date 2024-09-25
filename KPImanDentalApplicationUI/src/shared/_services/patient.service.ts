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

  getPatientsById(id: number): Observable<PatientDto> {
    const params = `Id=${id}`;
    return this.http.get<PatientDto>(this.apiUrl + 'GetPatientsById?' + params).pipe();
  }

  createOrUpdatePatient(patientDto: PatientDto) {
    return this.http.post(`${this.apiUrl}CreateOrUpdatePatient`, patientDto).pipe();
  }

  deletePatient(id: number) {
    const params = `Id=${id}`;
    return this.http.delete(`${this.apiUrl}/DeletePatient?${params}`).pipe();
  }

  getPatientTreatmentFormById(treatmentId : number): Observable<PatientTreamentFormDto> {
    return this.http.get<PatientTreamentFormDto>(this.apiUrl + 'GetPatientTreatmentFormById?TreatmentId=' + treatmentId ).pipe();
  }


}
