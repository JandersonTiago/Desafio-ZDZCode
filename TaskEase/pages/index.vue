<template>
  <div>
    <Header />
    <h1 class="task-h1">Gerenciador de Tarefas</h1>
    
    <form class="task-form">
      <div class="form-group">
        <label for="task-title">Título da Tarefa</label>
        <input type="text" id="task-title" v-model="taskTitle" />
      </div>
      
      <div class="form-group">
        <label for="task-date">Data</label>
        <input type="date" id="task-date" v-model="taskDate" />
      </div>
      
      <div class="form-group">
        <label for="task-status">Status</label>
        <select id="task-status" v-model="taskStatus">
          <option value="Em espera">Em Espera</option>
          <option value="Em processo">Em Processo</option>
          <option value="Concluido">Concluído</option>
        </select>
      </div>
      
      <div class="form-group">
        <label for="task-responsible">Responsável</label>
        <select id="task-responsible" v-model="taskResponsible">
          <option v-for="user in users" :key="user">{{ user }}</option>
        </select>
      </div>
      
      <button type="submit" @click.prevent="cadastrarTarefa">Cadastrar</button>
    </form>
    
    <table class="task-table">
      <thead>
        <tr>
          <th>ID Tarefa</th>
          <th>Título</th>
          <th>Data</th>
          <th>Status</th>
          <th>Responsável</th>
          <th>Atualizar</th>
          <th>Excluir</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="task in tasks" :key="task.id">
          <td>{{ task.id }}</td>
          <td>{{ task.titulo }}</td>
          <td>{{ formatarData(task.data) }}</td>
          <td>{{ task.status }}</td>
          <td>{{ task.responsavel }}</td>
          <td><button @click="atualizarTarefa(task)">Atualizar</button></td>
          <td><button class="delete-button" @click="excluirTarefa(task)">Excluir</button></td>
        </tr>
      </tbody>
    </table>
    
    <Footer />
  </div>
</template>

<script>
import Header from '~/components/navbar.vue'
import Footer from '~/components/footer.vue'
import axios from 'axios';

export default {
  components: {
    Header,
    Footer
  },
  data() {
    return {
      taskTitle: '',
      taskDate: '',
      taskStatus: '',
      taskResponsible: '',
      tasks: [],
      nextId: 1,
      users: []
    }
  },
  methods: {
    async cadastrarTarefa() {
      const newTask = {
        id: this.generateUniqueId(),
        titulo: this.taskTitle,
        data: this.taskDate,
        status: this.taskStatus,
        responsavel: this.taskResponsible
      }

      try {
        await axios.post('http://localhost:5159/Tarefas', newTask);
        this.tasks.push(newTask);
        this.taskTitle = '';
        this.taskDate = '';
        this.taskStatus = '';
        this.taskResponsible = '';
        alert('Tarefa cadastrada com sucesso!');
      } catch (error) {
        console.error('Erro ao cadastrar tarefa:', error);
        alert('Erro ao cadastrar tarefa. Por favor, tente novamente.');
      }
    },

    async buscarTarefas() {
      try {
        const response = await axios.get('http://localhost:5159/Tarefas');
        this.tasks = response.data;
      } catch (error) {
        console.error('Erro ao buscar tarefas:', error);
      }
    },

    generateUniqueId() {
      const currentDate = new Date();
      return parseInt(currentDate.getTime() / 1000);
    },

    async buscarUsuarios() {
      try {
        const response = await axios.get('http://localhost:5159/Usuario');
        this.users = response.data.map(user => user.nome);
      } catch (error) {
        console.error('Erro ao buscar usuários:', error);
      }
    },

    formatarData(data) {
      if (!data) return '';
      const dataObj = new Date(data);
      const dia = dataObj.getDate().toString().padStart(2, '0');
      const mes = (dataObj.getMonth() + 1).toString().padStart(2, '0');
      const ano = dataObj.getFullYear();
      return `${dia}/${mes}/${ano}`;
    },

    async excluirTarefa(task) {
      try {
        const response = await axios.delete(`http://localhost:5159/Tarefas/${task.id}`);
        if (response.status === 200) {
          this.tasks = this.tasks.filter(t => t.id !== task.id);
          console.log('Tarefa excluída com sucesso:', task);
        } else {
          console.error('Falha ao excluir tarefa:', response.data);
        }
      } catch (error) {
        console.error('Erro ao excluir tarefa:', error);
      }
    }
  },

  mounted() {
    this.buscarUsuarios();
    this.buscarTarefas();
  }
}
</script>

<style scoped>
.task-form, .task-table, .task-h1 {
  font-family: Arial, Helvetica, sans-serif;
}

.task-h1 {
  margin: 20px 0;
}

.task-form {
  margin: 20px 0;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
}

button {
  padding: 10px 15px;
  background-color: #0000FF;
  color: #fff;
  border: none;
  cursor: pointer;
  font-family: Arial, Helvetica, sans-serif;
}

button:hover {
  background-color: #000099;
}

.task-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.task-table th,
.task-table td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

.task-table th {
  background-color: #f2f2f2;
}

button.delete-button {
  background-color: #DC143C;
}

button.delete-button:hover {
  background-color: #B22222;
}
</style>
