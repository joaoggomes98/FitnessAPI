import { Injectable, signal } from '@angular/core';

export type ToastType = 'success' | 'error' | 'info' | 'warning';

export interface ToastMessage {
  message: string;
  type: ToastType;
  visible: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  toast = signal<ToastMessage | null>(null);

  show(message: string, type: ToastType = 'info') {
    this.toast.set({
      message,
      type,
      visible: true
    });

    setTimeout(() => {
      this.clear();
    }, 3000);
  }

  success(message: string) {
    this.show(message, 'success');
  }

  error(message: string) {
    this.show(message, 'error');
  }

  info(message: string) {
    this.show(message, 'info');
  }

  warning(message: string) {
    this.show(message, 'warning');
  }

  clear() {
    this.toast.set(null);
  }
}