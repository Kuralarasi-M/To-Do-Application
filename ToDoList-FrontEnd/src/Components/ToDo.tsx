import React, { useEffect, useState } from 'react'
import { FormControl, FormGroup, FormLabel, Table, Form, Button } from 'react-bootstrap'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { schema } from '../schema'
import type x from 'zod';
import axios from 'axios'
import AxiosInstance from '../api/loginAPI'
import { Navigate, useNavigate } from 'react-router-dom'

export type TodoType = x.infer<typeof schema>

const ToDo = () => {
    const { register, handleSubmit, formState: { errors }, reset,watch } = useForm<TodoType>({
        resolver: zodResolver(schema),
        defaultValues: {
            id: 0,
            title: "",
            description: "",
            createdDate: "",
            isCompleted:false
        },
    });

    const [todo, setTodo] = useState<TodoType[]>([]);
    const deleteToDo = async (todo: TodoType) => {
        const response = await AxiosInstance.delete(`/TodoApp/${todo.id}`)
            .then(() => setTodo(prev => prev.filter(t => t.id !== todo.id)))
            .catch(err => console.error(err));
          
        

    }
useEffect(() => {
  AxiosInstance.get("/TodoApp")
    .then((res) => {
      console.log(res.data);
      setTodo(res.data);
    })
    .catch((err) => {
      console.error("Error fetching todos:", err);
      
    });
}, []);

    const handleToDo = async (data: TodoType) => {
        const newTodo = {
            ...data,
           
            createdDate: data.id && data.id > 0
                ? data.createdDate
                : new Date().toISOString()
        };
        if (data.id && data.id > 0) {
            try {
                const response = await AxiosInstance.put(`/TodoApp/${data.id}`, newTodo);

                setTodo(prev => prev.map(t => t.id === data.id ? data : t));
                reset({ id: 0, title: "", description: "", createdDate: "" ,isCompleted:false});
                  console.log(data);
            } catch (err) {
                console.error(err);
            }
        }
        else {
            try {
                const response = await AxiosInstance.post("/TodoApp", newTodo);
                
                console.log(data);
                
                setTodo(prev => [...prev, data]);
                reset({ id: 0, title: "", description: "", createdDate: "" ,isCompleted:false});
            } catch (err) {
                console.error(err);
            }
        }


    }
   
    const editToDo = (todo: TodoType) => {
        reset(todo)
    }
    const currentId = watch('id');

    const token = localStorage.getItem('accessToken');

    const navigate = useNavigate();

   
   
    return (
        token ? <div className='m-5'>
            <div className='w-50 m-auto border p-5 rounded'>
                <Form onSubmit={handleSubmit(handleToDo)}>
                    <h2 className='m-auto text-primary text-center m-3'>Todo</h2>
                    <FormGroup>
                        <FormLabel>Created Date</FormLabel>
                        <FormControl
                            value={new Date().toISOString()}
                            disabled
                            {...register("createdDate")}
                        />
                    </FormGroup>

                    <FormGroup>
                        <FormLabel>Title</FormLabel>
                        <FormControl type="text" {...register("title")} isInvalid={!!errors.title} />
                        <FormControl.Feedback type="invalid">{errors.title?.message}</FormControl.Feedback>
                    </FormGroup>

                    <FormGroup>
                        <FormLabel>Description</FormLabel>
                        <FormControl type="text" {...register("description")} isInvalid={!!errors.description} />
                        <FormControl.Feedback type="invalid">{errors.description?.message}</FormControl.Feedback>
                    </FormGroup>
                    <FormGroup>
                        <Form.Group className="mb-3" controlId="todoCompleted">
          <Form.Check
            type="checkbox"
            label="Completed"
            {...register("isCompleted")}
          />
        </Form.Group>
                    </FormGroup>
                    <button type="submit" className="btn btn-primary mt-2">{(currentId && currentId>0)?"Update ToDo":"Add ToDo"}</button>
                </Form>
            </div>

            <div className='w-75 m-auto mt-3 rounded'>
                <Table className="table table-bordered table-striped table-hover rounded table-responsive table-primary ">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Title</th>
                            <th>Description</th>
                            <th>CreatedDate</th>
                            <th>IsCompleted</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {todo.map((a, index) => (
                            <tr key={index}>
                                <td>{a.id}</td>
                                <td>{a.title}</td>
                                <td>{a.description}</td>
                                <td>{new Date(a.createdDate).toLocaleString()}</td>
                                <td>{(a.isCompleted==true)?"Yes":"No"}</td>
                                <td>
                                    <Button className='mx-2 bg-warning' onClick={() => editToDo(a)}>Edit</Button>
                                    <Button className='mx-2 bg-danger' onClick={() => deleteToDo(a)}>Delete</Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </div>

        </div>
        :
        <Navigate to='/' />
    )
}

export default ToDo

