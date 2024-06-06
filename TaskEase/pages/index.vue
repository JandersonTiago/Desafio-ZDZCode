<template>
  <div>
    <Header />
    <h1 class="task-h1">Gerenciador de tarefas</h1>
    
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
          <option value="em espera">Em Espera</option>
          <option value="em processo">Em Processo</option>
          <option value="concluido">Concluído</option>
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
        <tr v-for="task in tasks" :key="task.id_tarefa">
          <td>{{ task.id_tarefa }}</td>
          <td>{{ task.title }}</td>
          <td>{{ task.date }}</td>
          <td>{{ task.status }}</td>
          <td>{{ task.responsible }}</td>
          <td><button @click="atualizarTarefa(task)">Atualizar</button></td>
          <td><button @click="excluirTarefa(task)">Excluir</button></td>
        </tr>
      </tbody>
    </table>
    
    <Footer />
  </div>
</template>


<script>
import Header from '~/components/navbar.vue'
import Footer from '~/components/footer.vue'

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
      nextId: 1, // Variável para gerar o próximo ID automaticamente
      users: [] // Array para armazenar os nomes dos usuários
    }
  },
  methods: {
    async cadastrarTarefa() {
      // Adiciona uma nova tarefa à lista
      const newTask = {
        id_tarefa: this.nextId,
        title: this.taskTitle,
        date: this.taskDate,
        status: this.taskStatus,
        responsible: this.taskResponsible
      }
      this.tasks.push(newTask)
      this.nextId++ // Incrementa o próximo ID

      // Reseta os campos do formulário
      this.taskTitle = ''
      this.taskDate = ''
      this.taskStatus = ''
      this.taskResponsible = ''
    },
    atualizarTarefa(task) {
      // Implemente a lógica para atualizar a tarefa
      console.log('Atualizando tarefa:', task);
    },
    excluirTarefa(task) {
      // Implemente a lógica para excluir a tarefa
      const index = this.tasks.indexOf(task);
      if (index !== -1) {
        this.tasks.splice(index, 1);
      }
    },
    async buscarUsuarios() {
      try {
        // Faz a requisição para a API que retorna os usuários
        const response = await this.$axios.get('http://localhost:5159/Usuario');
        
        // Extrai apenas os nomes dos usuários
        this.users = response.data.map(user => user.nome);
        
        console.log(this.users);

      } catch (error) {
        console.error('Erro ao buscar usuários:', error);
      }
    }
  },
  mounted() {
    this.buscarUsuarios(); // Chama a função para buscar os usuários ao montar o componente
  }
}
</script>


<style scoped>
/* Estilos específicos da página principal */
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
</style>
