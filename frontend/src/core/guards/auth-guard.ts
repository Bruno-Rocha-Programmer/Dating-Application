import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account-service';
import { ToastService } from '../services/toast-service';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toast = inject(ToastService);

  if (accountService.currentUser()) return true;
  else {
    toast.error('You need to login to view this page!');
    return false;
  }
};
