import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  loginForm: FormGroup

  constructor(private fb: FormBuilder){
    this.loginForm = this.fb.group({
      email: new FormControl('', {validators: [Validators.required]}),
      password: new FormControl('', {validators: [Validators.required]}),
    })
  }


  ngOnInit(): void {
   this.emailField.valueChanges.subscribe((value) => {
   })
  }


  get emailField(): FormControl {
		return this.loginForm.get( 'email' ) as FormControl;
	}

  get passwordField(): FormControl {
		return this.loginForm.get( 'password' ) as FormControl;
	}

  

  onSubmitForm(){
     // TODO: Use EventEmitter with form value
     console.warn(this.loginForm.value);
  }


}
