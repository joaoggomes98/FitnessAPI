import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CreateNutritionPlan } from '../models/nutrition-Plans/create-nutrition-plan';
import { NutritionPlan } from '../models/nutrition-Plans/nutrition-plan';

@Injectable({
  providedIn: 'root'
})
export class NutritionService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:5001/api/nutrition/';
  private baseUrlAux = 'https://localhost:5001/api/clients/';

  createNutritionPlan(model: CreateNutritionPlan) {
    return this.http.post(`${this.baseUrl}${model.clientId}`, model);

  }

  getNutritionPlan() {
    return this.http.get<NutritionPlan[]>(this.baseUrlAux + 'nutrition-plans')
  }

}
