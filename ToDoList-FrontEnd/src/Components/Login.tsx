// Login.tsx
import React, { useState } from 'react';
import '../App.css';
import { Button, Form, FormControl, FormGroup, FormLabel } from 'react-bootstrap';
import type z from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import axios from 'axios';
import loginSchema from './loginSchema';
import { useNavigate } from 'react-router-dom';

export type UserData = z.infer<typeof loginSchema>;

const Login = () => {
  const [loading, setLoading] = useState(false);
  const [apiError, setApiError] = useState('');
  const [action] = useState('Login');
  const navigate= useNavigate();
  

  const { register, handleSubmit, formState: { errors } } = useForm<UserData>({
    resolver: zodResolver(loginSchema),
    defaultValues: { email: '', password: '' }
  });

  const formSubmit = async (data: UserData) => {
    console.log("Form submitted!", data);
    setLoading(true);
    setApiError('');

    try {
 
      const res = await axios.post('https://localhost:7275/api/authentication', data);
      const { accessToken, refreshToken } = res.data;

      localStorage.setItem('accessToken', accessToken);
      localStorage.setItem('refreshToken', refreshToken);
     console.log(res.data);
      navigate('/dashboard');

      
     
    } catch (err: any) {
      if (err.response?.status === 401) {
        setApiError('Invalid credentials');
      } else {
        setApiError('Something went wrong. Please try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className='m-5'>
      <Form
        className='w-25 p-5 m-auto border rounded shadow-lg'
        onSubmit={handleSubmit(formSubmit)}
      >
        <h3 className='text-center mb-4'>{action}</h3>

        <FormGroup className='mb-3'>
          <FormLabel>Email</FormLabel>
          <FormControl
            type='email'
            {...register('email')}
            className='form-control p-2'
            isInvalid={!!errors.email}
          />
          <FormControl.Feedback type='invalid'>
            {errors.email?.message}
          </FormControl.Feedback>
        </FormGroup>

        <FormGroup className='mb-3'>
          <FormLabel>Password</FormLabel>
          <FormControl
            type='password'
            {...register('password')}
            className='form-control p-2'
            isInvalid={!!errors.password}
          />
          <FormControl.Feedback type='invalid'>
            {errors.password?.message}
          </FormControl.Feedback>
        </FormGroup>

        {apiError && <p className='text-danger'>{apiError}</p>}

        <Button type='submit' className='m-4' disabled={loading}>
          {loading ? 'Logging in...' : action}
        </Button>
      </Form>
    </div>
  );
};

export default Login;
