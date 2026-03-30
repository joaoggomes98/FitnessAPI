import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CreateTrainingPlan } from '../models/training-plans/create-training-plan';
import { TrainingPlan } from '../models/training-plans/training-plan';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:5001/api/training/';
  private baseUrlAux = 'https://localhost:5001/api/clients/';
  
  createTrainingPlan (model: CreateTrainingPlan){
    return this.http.post(`${this.baseUrl}${model.clientId}`, model);

  }

  getTrainingPlan(){
      return this.http.get<TrainingPlan[]>(this.baseUrlAux + 'training-plans')
    }

   
}
