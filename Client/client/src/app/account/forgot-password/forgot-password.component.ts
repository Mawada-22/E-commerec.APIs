import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

// Two-step reset: (1) request a token for the email, (2) set the new password.
// The token is shown/filled automatically in dev because the API returns it
// directly (no email sender configured yet).
@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html'
})
export class ForgotPasswordComponent {
  step = 1;
  errors: string[] = [];

  emailForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email])
  });

  resetForm = new FormGroup({
    email: new FormControl(''),
    token: new FormControl('', Validators.required),
    newPassword: new FormControl('', Validators.required)
  });

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router) {}

  requestToken() {
    this.errors = [];
    const email = this.emailForm.value.email;
    this.accountService.forgotPassword(email).subscribe(response => {
      this.resetForm.patchValue({email: response.email, token: response.token});
      this.step = 2;
      this.toastr.info('Reset code generated — set your new password below.');
    }, error => {
      this.errors = ['No account was found with this email.'];
    });
  }

  resetPassword() {
    this.errors = [];
    this.accountService.resetPassword(this.resetForm.value).subscribe(() => {
      this.toastr.success('Password changed! You can log in now.');
      this.router.navigateByUrl('/account/login');
    }, error => {
      this.errors = error.errors || [error.error?.message || 'Could not reset the password.'];
    });
  }
}
